# Cambios en el Frontend: búsqueda de clientes

## Resumen
Se agregó una página nueva `/clientes` con un buscador reactivo y una tarjeta de detalle. No se modificó la vista `ejemplo`.

## Archivos

| Archivo | Cambio |
|---|---|
| `frontend/src/app/features/clientes/clientes.page.ts` | Componente nuevo con la lógica de búsqueda |
| `frontend/src/app/features/clientes/clientes.page.html` | Plantilla nueva: input, lista de resultados y ficha |
| `frontend/src/app/app.routes.ts` | Ruta lazy `clientes` → `ClientesPage` |
| `frontend/src/app/core/layout/sidebar.component.html` | Link "Clientes" en el menú |

## Cómo funciona

### Buscador reactivo (`clientes.page.ts`)
`busqueda` es un `FormControl` cuyo `valueChanges` pasa por este pipeline:

1. `debounceTime(300)`: espera 300 ms sin escribir antes de continuar.
2. `distinctUntilChanged()`: ignora el valor si es igual al anterior.
3. `switchMap`:
   - Si el término (con `trim`) tiene menos de 3 caracteres, devuelve `[]` y no llama a la API.
   - Si tiene 3 o más, hace `GET {apiUrl}/clientes/buscar?q=...&limit=10`.
   - `switchMap` cancela la petición anterior si el usuario sigue escribiendo.
   - `catchError` muestra un mensaje en la señal `error` y devuelve `[]`.
4. `tap`: limpia el cliente seleccionado cuando llegan resultados nuevos.

`toSignal(...)` convierte el stream en la señal `resultados`, con valor inicial `[]`. Así la plantilla la lee como `resultados()` y no hace falta suscribirse a mano.

### Selección y detalle
- `seleccionado` es una `signal<Cliente | null>`.
- Cada resultado es un `<button>` que hace `seleccionado.set(c)`.
- `@if (seleccionado(); as c)` muestra la ficha con Razón Social, Tipo y Número de Documento, Segmento y Correo.

### Otros detalles
- `Cliente` es una interfaz exportada desde `clientes.page.ts`. Sus campos coinciden con el `ClienteDto` del backend en camelCase (`razonSocial`, `numeroDocumento`, etc.).
- La URL se arma con `environment.apiUrl` (`/api/v1`), igual que `EjemploHttpService`.
- Usa `HttpClient` directamente. No se creó una capa de servicio ni mock para mantener el cambio mínimo.
- El componente es `standalone` y solo importa `ReactiveFormsModule`.

## Verificación
`npx tsc --noEmit -p tsconfig.app.json` pasa sin errores. No se probó en navegador contra el backend.
