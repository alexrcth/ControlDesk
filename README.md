# ControlDesk

Sistema de tickets para soporte técnico desarrollado con ASP.NET Core, PostgreSQL, JWT y Vue 3.

---

# Tecnologías utilizadas

## Backend

* ASP.NET Core 8
* Entity Framework Core
* PostgreSQL
* JWT Authentication
* Swagger

## Frontend

* Vue 3
* Vite
* Axios
* Vue Router

---

# Funcionalidades

* Registro de usuarios
* Inicio de sesión con JWT
* Roles de usuario
* Protección de rutas con Authorize
* Gestión de tickets
* Dashboard de tickets
* Integración frontend + backend
* Conexión con PostgreSQL/Supabase

---

# Requisitos

## Backend

* .NET 8 SDK
* PostgreSQL
* Visual Studio Code o Visual Studio

## Frontend

* Node.js LTS
* npm

---

# Clonar proyecto

## Backend

```bash
git clone https://github.com/alexrcth/ControlDesk.git
```

## Frontend

```bash
git clone https://github.com/dulcevic-ZV/controldesk-project.git
```

---

# Configuración Backend

## Restaurar dependencias

```bash
dotnet restore
```

## Ejecutar backend

```bash
dotnet run
```

Backend disponible en:

```txt
http://localhost:5215
```

Swagger:

```txt
http://localhost:5215/swagger
```

---

# Configuración Frontend

## Instalar dependencias

```bash
npm install
```

## Configurar archivo .env

Crear un archivo `.env` en la raíz del frontend:

```env
VITE_API_URL=http://localhost:5215/api
```

## Ejecutar frontend

```bash
npm run dev
```

Frontend disponible en:

```txt
http://localhost:5173
```

---

# Configuración CORS

El backend tiene habilitado CORS para:

```txt
http://localhost:5173
```

---

# Roles del sistema

## Roles disponibles

* ADMIN
* SUPPORT_AGENT
* CLIENT

---

# Estructura del proyecto

## Backend

```txt
Application/
Controllers/
Domain/
Infrastructure/
```

## Frontend

```txt
src/
 ├── components/
 ├── services/
 ├── router/
 ├── views/
 └── stores/
```

---

# Autenticación JWT

El sistema utiliza JWT para autenticación y autorización.

El token se guarda en localStorage y se envía automáticamente mediante Axios Interceptors.

---

# Variables importantes

## Backend

Configuradas en `appsettings.json`:

```json
"Jwt": {
  "Key": "YOUR_SECRET_KEY",
  "Issuer": "ControlDeskAPI",
  "Audience": "ControlDeskUsers"
}
```

## Frontend

Configuradas en `.env`:

```env
VITE_API_URL=http://localhost:5215/api
```

---

# Integrantes

* Alexander Rodriguez
* Dulce
* Angel

---

# Estado del proyecto

Proyecto funcional con:

* Login JWT
* Roles
* Conexión frontend/backend
* PostgreSQL/Supabase
* Gestión básica de tickets

---

# Licencia

Proyecto académico.
