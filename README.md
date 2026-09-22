# ☕ Desayuno Asíncrono


Práctica de 2º DAW: automatizar la preparación de un desayuno de 7 acciones, comparando distintas formas de ejecutarlas — desde el de modelo visto en 1º hasta la que realmente aprovecha el paralelismo.

## 🍳 Las 7 acciones

| # | Acción | Tiempo | Depende de |
|---|---|---|---|
| 1 | Café | 200ms | — |
| 2 | Sartén | 200ms | — |
| 3 | Huevos | 300ms | Sartén caliente |
| 4 | Bacon | 300ms | Sartén caliente |
| 5 | Pan | 200ms | — |
| 6 | Mantequilla | 100ms | Pan tostado |
| 7 | Zumo | 200ms | — |

Restricción: el desayuno tiene que estar listo en **500ms** o se enfría el café. 🥶

## ⏱️ Tabla de tiempos

| Enfoque | Teórico | Medido | Con timeout (500ms) |
|---|---|---|---|
| Secuencial | 1500ms | 1552ms | ❌ Cortado |
| Async/Await | 1500ms | 1548ms | ❌ Cortado |
| Mejor rendimiento | 500ms | 508ms | ❌ Cortado (por solo 8ms) |

## 🤔 Preguntas de reflexión

### 1. ¿Qué diferencias has observado entre las soluciones?

**Secuencial**: un único hilo hace las 7 acciones una detrás de otra, bloqueado en cada una (`.Wait()`). Sumas los tiempos y ya está: 1500ms.

**Async/await**: seguimos con un único hilo, pero en vez de quedarse bloqueado, se queda libre para otro usuario mientras espera. El problema es que aquí eso no sirve de nada — las acciones se siguen lanzando una detrás de otra, así que tarda prácticamente lo mismo que la secuencial (1548ms). `await` no es sinónimo de paralelo.

**Mejor rendimiento**: aquí sí cambia la cosa. En vez de esperar cada acción antes de lanzar la siguiente, lanzamos ya mismo todo lo que no depende de nada (café, sartén, pan, zumo), sin esperarlas una a una. En cuanto la sartén está lista, lanzamos huevos y bacon a la vez; en cuanto el pan está tostado, lanzamos la mantequilla. Solo esperamos todo junto al final con `Task.WhenAll`. El tiempo ya no es la suma, es el camino más largo de dependencias: sartén + huevos/bacon = 500ms.

### 2. ¿Qué acciones se pueden ejecutar a la vez y cuáles no? ¿Por qué?

Café, sartén, pan y zumo van todas a la vez desde el minuto 0 — no necesitan nada de las demás. Huevos y bacon esperan a que la sartén esté caliente, pero entre ellos no se necesitan, así que también van en paralelo. Mantequilla espera al pan tostado.

### 3. ¿Qué ha pasado con cada solución cuando introduces el timeout?

Las tres se cortan al pasar los 500ms — hasta la de mejor rendimiento, que se pasa por solo 8ms (508ms medidos en el mejor de los casos).

### 4. ¿El enfoque con mejor rendimiento es también el más seguro? ¿Por qué?

No tengo claro que sea el más seguro — la pregunta me ha resultado un poco ambigua y no he terminado de entenderla del todo (¿"seguro" en tiempo? ¿en integridad de datos?). Lo que sí tengo claro es que es el más rápido y el que mejor aprovecha el paralelismo.

### 5. ¿Merece la pena complicarse con paralelismo o con mecanismos de control de tiempo? Justifica tu respuesta.

Creo que sí. Aunque ninguna solución llega a cumplir el límite, pasar de 1500ms a 500ms es rebajar el tiempo un 66% — y un 66% es muchísimo, se aplique donde se aplique (velocidad de un coche, impuestos, sueldo...).


## 🛠️ Decisión de diseño: `PrepararMejorRendimientoAsync`

Las dependencias entre acciones (huevos/bacon → sartén, mantequilla → pan) están escritas a mano, por nombre, dentro del propio método — no en un campo genérico del modelo `Accion`.

Esto va **a propósito** en contra del principio Abierto/Cerrado (SOLID): añadir una acción nueva con otra dependencia obligaría a tocar código, no solo datos. Decisión consciente: son 7 acciones fijas de un ejercicio puntual, no se va a reutilizar con otro desayuno — una solución genérica habría metido complejidad de más sin necesidad real (YAGNI).

## ▶️ Cómo ejecutarlo

```bash
cd DesayunoAsincrono
dotnet run
```

Y para los tests:

```bash
dotnet test
```
