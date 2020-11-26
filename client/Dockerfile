FROM node:8 as qnify-build

# Create app directory
WORKDIR /usr/src/qnify-webapp

# Install app dependencies
# A wildcard is used to ensure both package.json AND package-lock.json are copied
# where available (npm@5+)
COPY package*.json ./

# Bundle app source
COPY . .

RUN npm install
# If you are building your code for production
# RUN npm install --only=production

RUN npm run build-prod

# Stage 2 - the production environment
FROM nginx:1.15.8-alpine
COPY nginx.conf /etc/nginx/nginx.conf
COPY ./build /var/www
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]