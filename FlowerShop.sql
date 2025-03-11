-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Хост: 127.0.0.1:3306
-- Время создания: Мар 12 2025 г., 00:04
-- Версия сервера: 5.7.39
-- Версия PHP: 8.2.20

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `FlowerShop`
--

-- --------------------------------------------------------

--
-- Структура таблицы `cart`
--

CREATE TABLE `cart` (
  `CartID` int(11) NOT NULL,
  `UserID` int(11) NOT NULL,
  `ProductID` int(11) NOT NULL,
  `Quantity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Структура таблицы `categories`
--

CREATE TABLE `categories` (
  `CategoryID` int(11) NOT NULL,
  `Name` varchar(256) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Src` varchar(256) COLLATE utf8mb4_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Дамп данных таблицы `categories`
--

INSERT INTO `categories` (`CategoryID`, `Name`, `Src`) VALUES
(1, 'Свежесрезанные цветы', 'fresh_cut.jpg'),
(2, 'Цветочные букеты', 'flower_bouquets.jpg'),
(3, 'Комнатные растения', 'indoor_plants.jpg'),
(4, 'Рассада и семена', 'seedings.jpg'),
(5, 'Цветочные аксессуары', 'flower_accessories.jpg'),
(6, 'Садовые растения', 'garden_plants.jpg');

-- --------------------------------------------------------

--
-- Структура таблицы `orderitems`
--

CREATE TABLE `orderitems` (
  `OrderItemID` int(11) NOT NULL,
  `OrderID` int(11) NOT NULL,
  `ProductID` int(11) NOT NULL,
  `Quantity` int(11) NOT NULL,
  `Price` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Дамп данных таблицы `orderitems`
--

INSERT INTO `orderitems` (`OrderItemID`, `OrderID`, `ProductID`, `Quantity`, `Price`) VALUES
(1, 1, 1, 6, '150.00');

-- --------------------------------------------------------

--
-- Структура таблицы `orders`
--

CREATE TABLE `orders` (
  `OrderID` int(11) NOT NULL,
  `UserID` int(11) NOT NULL,
  `TotalAmount` decimal(10,2) NOT NULL,
  `Discount` decimal(10,2) NOT NULL,
  `FinalAmount` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Дамп данных таблицы `orders`
--

INSERT INTO `orders` (`OrderID`, `UserID`, `TotalAmount`, `Discount`, `FinalAmount`) VALUES
(1, 31, '900.00', '0.00', '900.00');

-- --------------------------------------------------------

--
-- Структура таблицы `products`
--

CREATE TABLE `products` (
  `ProductID` int(11) NOT NULL,
  `Name` varchar(256) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Price` decimal(10,2) NOT NULL,
  `Src` varchar(256) COLLATE utf8mb4_unicode_ci NOT NULL,
  `CategoryID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Дамп данных таблицы `products`
--

INSERT INTO `products` (`ProductID`, `Name`, `Price`, `Src`, `CategoryID`) VALUES
(1, 'Роза красная', '150.00', 'red_rose.jpg', 1),
(2, 'Тюльпан желтый', '100.00', 'tyulpan.jpg', 1),
(3, 'Гвоздика белая', '80.00', 'gvzd.jpg', 1),
(4, 'Лилия розовая', '200.00', 'liliya.jpg', 1),
(5, 'Орхидея фиолетовая', '250.00', 'orkhideya.jpg', 1),
(6, 'Хризантема белая', '90.00', 'xrizantemy.jpg', 1),
(7, 'Букет \"Романтика\"', '1000.00', 'romantic.jpg', 2),
(8, 'Букет \"Весеннее утро\"', '1200.00', 'spring_morning.jpg', 2),
(11, 'Букет \"Теплый вечер\"', '1100.00', 'afthermoon.jpg', 2),
(12, 'Букет \"Нежность\"', '950.00', 'nezhnost.jpg', 2),
(13, 'Букет \"Яркие краски\"', '1300.00', 'kraski.jpg', 2),
(14, 'Букет \"Ночное небо\"', '1050.00', 'night.jpg', 2),
(15, 'Фикус Бенджамина', '700.00', 'fikus.jpg', 3),
(16, 'Сансевиерия', '500.00', 'sansevieriya.jpg', 3),
(17, 'Спатифиллум', '600.00', 'spatfillum.jpg', 3),
(18, 'Антуриум', '800.00', 'anturium.jpg', 3),
(19, 'Кактус микс', '400.00', 'miks.jpg', 3),
(20, 'Драцена', '750.00', 'dratsena.jpg', 3),
(21, 'Рассада помидоров', '50.00', 'tomato.jpg', 4),
(22, 'Рассада огурцов', '40.00', 'cucumber.jpg', 4),
(23, 'Семена петрушки', '30.00', 'petrushka.jpg', 4),
(24, 'Семена укропа', '20.00', 'ukrop.jpg', 4),
(25, 'Семена салата', '25.00', 'salat.jpg', 4),
(26, 'Рассада перца', '60.00', 'perets.jpg', 4),
(27, 'Горшок керамический', '200.00', 'gorshok.jpg', 5),
(28, 'Подставка для цветов', '300.00', 'podstavka.jpg', 5),
(29, 'Удобрение для растений', '150.00', 'udobrenie.jpg', 5),
(30, 'Опора для растений', '100.00', 'opora.jpg', 5),
(31, 'Лейка для цветов', '250.00', 'leyka.jpg', 5),
(32, 'Флористическая губка', '80.00', 'gubka.jpg', 5),
(33, 'Садовая роза', '350.00', 'sadrose.jpg', 6),
(34, 'Сирень обыкновенная', '400.00', 'siren.jpg', 6),
(35, 'Пион древовидный', '500.00', 'pion.jpg', 6),
(36, 'Хоста', '300.00', 'hosta.jpg', 6),
(37, 'Гортензия', '450.00', 'gortenziya.jpg', 6),
(38, 'Жасмин', '380.00', 'zhasmin.jpg', 6);

-- --------------------------------------------------------

--
-- Структура таблицы `users`
--

CREATE TABLE `users` (
  `UserID` int(11) NOT NULL,
  `Email` varchar(256) COLLATE utf8mb4_unicode_ci NOT NULL,
  `FirstName` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `LastName` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `MiddleName` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `PhoneNumber` varchar(12) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `BirthDate` date DEFAULT NULL,
  `Street` varchar(256) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `HouseNumber` varchar(256) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `ApartmentNumber` varchar(256) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `EntranceNumber` int(11) DEFAULT NULL,
  `FloorNumber` int(11) DEFAULT NULL,
  `Password` varchar(256) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Gender` enum('Мужской','Женский') COLLATE utf8mb4_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Дамп данных таблицы `users`
--

INSERT INTO `users` (`UserID`, `Email`, `FirstName`, `LastName`, `MiddleName`, `PhoneNumber`, `BirthDate`, `Street`, `HouseNumber`, `ApartmentNumber`, `EntranceNumber`, `FloorNumber`, `Password`, `Gender`) VALUES
(31, 'lewis@gmail.com', 'Рустам', 'Шайхилисламов', 'Маратович', '89519488260', '2005-07-05', 'Городецкая', '6', '0', 0, 0, 'Lewis!123', 'Мужской');

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `cart`
--
ALTER TABLE `cart`
  ADD PRIMARY KEY (`CartID`),
  ADD KEY `FK_Cart_Users` (`UserID`),
  ADD KEY `FK_Cart_Products` (`ProductID`);

--
-- Индексы таблицы `categories`
--
ALTER TABLE `categories`
  ADD PRIMARY KEY (`CategoryID`);

--
-- Индексы таблицы `orderitems`
--
ALTER TABLE `orderitems`
  ADD PRIMARY KEY (`OrderItemID`),
  ADD KEY `FK_OrderItems_Orders` (`OrderID`),
  ADD KEY `FK_OrderItems_Products` (`ProductID`);

--
-- Индексы таблицы `orders`
--
ALTER TABLE `orders`
  ADD PRIMARY KEY (`OrderID`),
  ADD KEY `FK_Orders_Users` (`UserID`);

--
-- Индексы таблицы `products`
--
ALTER TABLE `products`
  ADD PRIMARY KEY (`ProductID`),
  ADD KEY `FK_Products_Categories` (`CategoryID`);

--
-- Индексы таблицы `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`UserID`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- AUTO_INCREMENT для сохранённых таблиц
--

--
-- AUTO_INCREMENT для таблицы `cart`
--
ALTER TABLE `cart`
  MODIFY `CartID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT для таблицы `categories`
--
ALTER TABLE `categories`
  MODIFY `CategoryID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT для таблицы `orderitems`
--
ALTER TABLE `orderitems`
  MODIFY `OrderItemID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT для таблицы `orders`
--
ALTER TABLE `orders`
  MODIFY `OrderID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT для таблицы `products`
--
ALTER TABLE `products`
  MODIFY `ProductID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=39;

--
-- AUTO_INCREMENT для таблицы `users`
--
ALTER TABLE `users`
  MODIFY `UserID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=32;

--
-- Ограничения внешнего ключа сохраненных таблиц
--

--
-- Ограничения внешнего ключа таблицы `cart`
--
ALTER TABLE `cart`
  ADD CONSTRAINT `FK_Cart_Products` FOREIGN KEY (`ProductID`) REFERENCES `products` (`ProductID`),
  ADD CONSTRAINT `FK_Cart_Users` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`);

--
-- Ограничения внешнего ключа таблицы `orderitems`
--
ALTER TABLE `orderitems`
  ADD CONSTRAINT `FK_OrderItems_Orders` FOREIGN KEY (`OrderID`) REFERENCES `orders` (`OrderID`),
  ADD CONSTRAINT `FK_OrderItems_Products` FOREIGN KEY (`ProductID`) REFERENCES `products` (`ProductID`);

--
-- Ограничения внешнего ключа таблицы `orders`
--
ALTER TABLE `orders`
  ADD CONSTRAINT `FK_Orders_Users` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`);

--
-- Ограничения внешнего ключа таблицы `products`
--
ALTER TABLE `products`
  ADD CONSTRAINT `FK_Products_Categories` FOREIGN KEY (`CategoryID`) REFERENCES `categories` (`CategoryID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
