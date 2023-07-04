CREATE DATABASE db_rental_car;

USE db_rental_car;

CREATE TABLE tb_cars
(
	car_id INT PRIMARY KEY IDENTITY(1,1),
	car_name VARCHAR(100) /*NOT NULL*/ UNIQUE,
	model VARCHAR(100) /*NOT NULL*/,
	make VARCHAR(100) /*NOT NULL*/,
	picture VARCHAR(MAX) /*NOT NULL*/,
	rent_price DECIMAL(7,2) /*NOT NULL*/,
	description VARCHAR(250)
);


CREATE TABLE tb_users
(
	user_id INT PRIMARY KEY IDENTITY(1,1),
	user_name VARCHAR(100) /*NOT NULL*/,
	contact_num VARCHAR(15) UNIQUE /*NOT NULL*/,
	email_address VARCHAR(100) UNIQUE /*NOT NULL*/,
	user_password VARCHAR(20) /*NOT NULL*/,
	user_address VARCHAR(250) /*NOT NULL*/,
	gender VARCHAR(10) /*NOT NULL*/ CHECK (gender IN ('male', 'female')),
	role VARCHAR(10) NOT NULL CHECK (role IN ('admin', 'user')),
);

CREATE TABLE tb_bookings
(
	booking_id INT PRIMARY KEY IDENTITY(1,1),
	user_id INT /*NOT NULL*/,
	car_booked INT /*NOT NULL*/,
	pickup_location VARCHAR(100) /*NOT NULL*/,
	return_location VARCHAR(100) /*NOT NULL*/,
	pickup_datetime DATETIME /*NOT NULL*/,
	return_datetime DATETIME /*NOT NULL*/,
	sub_total DECIMAL(7,2) /*NOT NULL*/,
	booked_at DATETIME /*NOT NULL*/ DEFAULT GETDATE(),
	FOREIGN KEY(user_id) REFERENCES tb_USERS(user_id),
	FOREIGN KEY(car_booked) REFERENCES tb_cars(car_id)
);