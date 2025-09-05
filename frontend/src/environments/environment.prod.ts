export const environment = {
  production: true,
  // En producción dentro de Docker/Nginx usar el nombre del servicio backend (docker-compose)
  // Si usas docker-compose y el servicio backend se llama `backend` y escucha en 8080 dentro de la red:
  // apiUrl: 'http://backend:8080/api'
  // Si vas a desplegar en un servidor real, reemplaza por la URL pública: https://api.tudominio.com/api
  apiUrl: '/api'
};
