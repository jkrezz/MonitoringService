FROM node:20-alpine
WORKDIR /app

COPY package.json package-lock.json ./
RUN npm ci

COPY . .

# Angular dev server with proxy to webapi
EXPOSE 4300
CMD ["npm", "run", "start", "--", "--host", "0.0.0.0", "--port", "4300", "--proxy-config", "proxy.conf.json"]

