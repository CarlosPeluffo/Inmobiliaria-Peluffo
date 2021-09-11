-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 11-09-2021 a las 10:15:08
-- Versión del servidor: 10.4.20-MariaDB
-- Versión de PHP: 8.0.9

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliaria`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `id_contrato` int(11) NOT NULL,
  `fecha_inicio` date NOT NULL,
  `fecha_fin` date NOT NULL,
  `monto` double NOT NULL,
  `cancelado` tinyint(4) NOT NULL,
  `fecha_cancelado` date DEFAULT NULL,
  `id_inquilino` int(11) NOT NULL,
  `id_inmueble` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`id_contrato`, `fecha_inicio`, `fecha_fin`, `monto`, `cancelado`, `fecha_cancelado`, `id_inquilino`, `id_inmueble`) VALUES
(5, '2021-08-27', '2021-10-28', 25000, 0, NULL, 2, 2),
(6, '2021-08-31', '2021-11-28', 25000, 0, NULL, 1, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `id_inmueble` int(11) NOT NULL,
  `id_propietario` int(11) NOT NULL,
  `direccion` varchar(200) NOT NULL,
  `uso` varchar(200) NOT NULL,
  `tipo` varchar(200) NOT NULL,
  `cant_ambientes` int(11) NOT NULL,
  `precio` double NOT NULL,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`id_inmueble`, `id_propietario`, `direccion`, `uso`, `tipo`, `cant_ambientes`, `precio`, `estado`) VALUES
(1, 1, 'Potosi 375', 'Residencial', 'Casa', 5, 1500000, 1),
(2, 1, 'Potosi 399', 'Residencial', 'Casa', 6, 1500000, 1),
(4, 1, 'Algo 123', 'Comercial', 'Salón', 1, 50000, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `id_inquilino` int(11) NOT NULL,
  `apellido` varchar(200) NOT NULL,
  `nombre` varchar(200) NOT NULL,
  `dni` varchar(12) NOT NULL,
  `mail` varchar(200) DEFAULT NULL,
  `telefono` varchar(200) NOT NULL,
  `lugar_trabajo` varchar(200) NOT NULL,
  `dni_garante` varchar(12) NOT NULL,
  `nombre_garante` varchar(200) NOT NULL,
  `telefono_garante` varchar(200) NOT NULL,
  `mail_garante` varchar(200) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`id_inquilino`, `apellido`, `nombre`, `dni`, `mail`, `telefono`, `lugar_trabajo`, `dni_garante`, `nombre_garante`, `telefono_garante`, `mail_garante`) VALUES
(1, 'Aguilar', 'Mario Oscar', '34576990', 'marito@gmail.com', '2657796846', 'Jugos Tang', '32450976', 'Scrimaglia Alejandro', '2664578230', 'alejandroS@gmail-com'),
(2, 'Mefisto', 'Magnus', '35738534', 'magnus_mefisto@gmail.com', '01163859471', 'Mefisto Ink.', '32459854', 'Marito Baracus', '01165733001', 'marito_baracus@yahoo.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `id_pago` int(11) NOT NULL,
  `nro_pago` int(11) NOT NULL,
  `fecha_pago` date NOT NULL,
  `monto` double NOT NULL,
  `id_contrato` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`id_pago`, `nro_pago`, `fecha_pago`, `monto`, `id_contrato`) VALUES
(1, 1, '2021-09-02', 25000, 6),
(4, 2, '2021-09-02', 25004, 6),
(9, 3, '2021-09-03', 25010, 6),
(11, 1, '2021-09-07', 24003, 5);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `id_propietario` int(11) NOT NULL,
  `apellido` varchar(200) NOT NULL,
  `nombre` varchar(200) NOT NULL,
  `dni` varchar(12) NOT NULL,
  `mail` varchar(200) DEFAULT NULL,
  `telefono` varchar(200) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`id_propietario`, `apellido`, `nombre`, `dni`, `mail`, `telefono`) VALUES
(1, 'Peluffo', 'Carlos Ayrton', '37505706', 'carlossolari1994@gmail.com', '2657302766');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `id_usuario` int(11) NOT NULL,
  `nombre` varchar(200) NOT NULL,
  `apellido` varchar(200) NOT NULL,
  `mail` varchar(200) NOT NULL,
  `clave` varchar(250) NOT NULL,
  `avatar` varchar(200) DEFAULT NULL,
  `rol` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`id_usuario`, `nombre`, `apellido`, `mail`, `clave`, `avatar`, `rol`) VALUES
(2, 'Admin', 'Admin', 'admin@admin.com', 'rfCzKZ8yCB6TsDT6RQyjXL5esg01QEXyY2Qoqp2ywBI=', '/UsersFiles\\photo_2.png', 1),
(3, 'Sampler', 'Momo', 'rey_momo@gmail.com', 'oGul7LO4P4tcN5bk1haHIkE2QMvhY4Tcn5F5vVQf1Ps=', '/UsersFiles\\photo_3.jpg', 0);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`id_contrato`),
  ADD KEY `id_inquilino` (`id_inquilino`),
  ADD KEY `id_inmueble` (`id_inmueble`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`id_inmueble`),
  ADD UNIQUE KEY `direccion` (`direccion`),
  ADD KEY `id_propietario` (`id_propietario`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`id_inquilino`),
  ADD UNIQUE KEY `dni` (`dni`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`id_pago`),
  ADD UNIQUE KEY `nro_pago` (`nro_pago`,`id_contrato`),
  ADD KEY `id_contrato` (`id_contrato`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`id_propietario`),
  ADD UNIQUE KEY `dni` (`dni`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`id_usuario`),
  ADD UNIQUE KEY `mail` (`mail`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `id_contrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `id_inmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `id_inquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `id_pago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `id_propietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id_usuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `contratos_ibfk_1` FOREIGN KEY (`id_inquilino`) REFERENCES `inquilinos` (`id_inquilino`),
  ADD CONSTRAINT `contratos_ibfk_2` FOREIGN KEY (`id_inmueble`) REFERENCES `inmuebles` (`id_inmueble`);

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `inmuebles_ibfk_1` FOREIGN KEY (`id_propietario`) REFERENCES `propietarios` (`id_propietario`);

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `pagos_ibfk_1` FOREIGN KEY (`id_contrato`) REFERENCES `contratos` (`id_contrato`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
