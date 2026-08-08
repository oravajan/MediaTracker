FROM node:24-alpine AS build
WORKDIR /app

RUN npm install -g pnpm

COPY package.json pnpm-lock.yaml ./
RUN echo "node-linker=hoisted" > .npmrc
RUN pnpm install --frozen-lockfile

COPY . .
RUN VITE_API_BASE_URL="" pnpm build

FROM nginx:alpine AS runtime
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80