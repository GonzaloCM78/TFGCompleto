# Usa una imagen base de Java 17
FROM eclipse-temurin:17

# Directorio de trabajo en el contenedor
WORKDIR /app

# Copia el archivo de configuración del proyecto y descarga dependencias
COPY pom.xml .
RUN apt-get update && apt-get install -y maven
RUN mvn dependency:go-offline

# Copia el resto del código
COPY . .

# Compila el proyecto
RUN mvn clean package spring-boot:repackage -DskipTests


# Expón el puerto de Spring Boot
EXPOSE 8080

# Ejecuta el .jar generado
CMD java -jar target/frogger-0.0.1-SNAPSHOT.jar

