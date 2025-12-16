# Build stage
FROM node:20-alpine AS Build
WORKDIR /app

COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build -- --configuration production

# Runtime stage
FROM nginx:alpine

# SPA fallback (Angular routing)
COPY nginx.conf /etc/nginx/conf.d/default.conf

COPY --from=build /app/dist/music-library-ui/browser /usr/share/nginx/html

EXPOSE 80