# Cambio: se elimina `moduleContentTypeCode` de contenido de módulos

Ningún endpoint cambia de ruta ni de verbo. Solo cambia la **forma** de los request/response de los 4 endpoints existentes de contenido (admin) y del detalle de región del Aprendiz (que reutiliza el mismo modelo de contenido).

## Por qué

Antes, cada bloque de contenido de un módulo tenía un `moduleContentTypeCode` (`MODULE_CONTENT_TYPE_TEXT` / `_VIDEO` / `_DOCUMENT` / `_IMAGE` / `_LINK`) que en teoría determinaba cómo renderizarlo. En la práctica esa distinción ya no aporta nada del lado del backend — no se usa para validar ni para nada — y con el enfoque de HTML libre + Prediseños, un mismo bloque puede tener texto, una o varias imágenes, un documento, etc. todo junto. El tipo pasó a ser 100% una decisión del frontend, no algo que el backend deba declarar.

## Endpoints afectados (sin cambio de ruta)

```
GET    courses/{courseId}/content                                          → CourseContentResponse
GET    courses/{courseId}/content/versions/{versionId}                     → VersionDetailResponse
POST   courses/{courseId}/content/course_modules/{moduleId}/module_contents → number (contentId)
PUT    courses/{courseId}/content/module_contents/{contentId}              → void
POST   courses/{courseId}/content/module_contents/{contentId}/files        → ApiResponse<string> (sin cambios)

GET    aprendiz/regiones/{id}                                              → AprendizRegionDetail (reutiliza el mismo modelo)
```

## Contrato nuevo

### `AddModuleContentRequest` (antes tenía `moduleContentTypeCode` obligatorio)
```ts
interface AddModuleContentRequest {
  text?: string | null;
  designJson?: string | null;
}
```

### `EditModuleContentRequest` — sin cambios
```ts
interface EditModuleContentRequest {
  text?: string | null;
  designJson?: string | null;
}
```

### `ModuleContentItem` / `ContentResponse` (admin) y `AprendizModuleContentResponse` (Aprendiz) — ya no traen `moduleContentTypeCode`
```ts
interface ModuleContentItem {
  id: number;
  text: string | null;
  designJson: string | null;
  orderIndex: number;
  files: ContentFile[];
}
```
*(antes tenía `moduleContentTypeCode: string;` entre `id` y `text` — ese campo desaparece por completo.)*

`ContentFile` no cambia:
```ts
interface ContentFile {
  id: number;
  fileTypeCode: string;
  fileUrl: string;
  fileName: string;
  orderIndex: number;
}
```

## Qué debe hacer el frontend ahora

- **Dejar de mandar `moduleContentTypeCode`** en el `POST .../module_contents` — si lo sigue mandando, el backend simplemente lo ignora (el campo ni siquiera existe en el DTO), no da error, pero es dead weight.
- **Dejar de leer `moduleContentTypeCode`** de las respuestas — ya no viene.
- **Cada bloque es ahora uniforme**: `text` (HTML libre) es la fuente de verdad de qué se muestra. El admin arma el bloque con el editor rico e inserta `<img src="...">`, un link de descarga, un `<video>`, etc. usando la URL que le devuelve `POST .../files` al subir cada archivo — el mismo endpoint de subida de siempre, sin cambios.
- **`designJson`** sigue siendo opuesto/paralelo a `text`: viene poblado (string opaco, mismo formato `ValueNode[]` ya acordado) cuando el bloque se armó eligiendo un Prediseño; `null` cuando es texto libre sin prediseño. El backend no lo interpreta, solo lo guarda y devuelve tal cual.
- `files[]` en cada bloque sigue existiendo igual que antes (0..N archivos por bloque, sin relación con ningún tipo) — ahí están todos los archivos subidos a ese bloque en particular, por si el frontend necesita la lista completa además de las URLs ya insertadas en el HTML.

## Nota sobre datos existentes

Si ya hay contenido guardado con `moduleContentTypeCode`, esa columna se elimina de la base de datos (la migración la borra). El HTML/`text` y los archivos existentes no se tocan — solo desaparece la columna de tipo.
