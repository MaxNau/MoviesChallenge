FROM lodgify/movies-api:3
WORKDIR /app
RUN mv amovies-db.json movies-db.json