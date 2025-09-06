# App Autenticación Full Stack

Este proyecto es una aplicación **Full Stack** desarrollada con:
- **Backend:** .NET 8 con Entity Framework Core (API REST)
- **Frontend:** Angular 17
- **Base de datos:** SQL Server (usando Docker o SQL Server Management Studio)
- **Contenedores:** Docker Compose (opcional)

## Funcionalidades principales
- **Autenticación con JWT**
- **Registro de usuarios** (rol `user` o `admin`)
  - Para crear un `admin`, es necesario ingresar una contraseña de creación especial.
- **Login seguro** con bloqueo progresivo:
  - 3 intentos fallidos → bloqueo 1 minuto
  - 1 intento más fallido → bloqueo 5 minutos
  - Si se repite, bloqueo indefinido hasta que el admin lo desbloquee
- **CRUD de usuarios (solo admin)**
- **Perfil de usuario (user y admin)**:
  - Cambiar nombre y contraseña
  - Confirmación previa antes de cambios
- **Manejo offline amigable**: si no hay conexión, aparece el mensaje **“Buscando red…”**
- **Interfaz separada en rutas**:
  - `/login`
  - `/register`
  - `/users` (solo admin)
  - `/users/new` (solo admin)
  - `/users/:id/edit` (solo admin)
  - `/profile`

## 🗂️ Estructura del proyecto
