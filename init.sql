-- phpMyAdmin SQL Dump
-- version 5.1.2
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Apr 18, 2024 at 07:06 PM
-- Server version: 5.7.24
-- PHP Version: 8.0.1

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `itransition_task4`
--

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE itransition_task4_iixn.users (
  `id` varchar(64) NOT NULL,
  `name` varchar(32) NOT NULL,
  `hashPassword` varchar(64) NOT NULL,
  `email` varchar(64) NOT NULL,
  `role` varchar(16) NOT NULL,
  `registrationtime` datetime NOT NULL,
  `lastLogintime` datetime NOT NULL,
  `isblocked` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `users`
--

INSERT INTO itransition_task4_iixn.users (`id`, `name`, `hashPassword`, `email`, `role`, `registrationtime`, `lastLogintime`, `isblocked`) VALUES
('323cd399aa75404eb48515662d29dbb4', 'Muhammad', '67B176705B46206614219F47A05AEE7AE6A3EDBE850BBBE214C536B989AEA4D2', 'muhammadarch22@gmail.com', 'admin', '2024-04-18 11:31:43', '2024-04-18 11:31:47', 0),
('4e2940475c0f42bcbf11650784febb02', 'Muhammad', '77ED20D9EA0DF1C32D48E20C432D833738479038C7DBC6EF37CC5EFE3C5AA920', 'muh22@gmail.cim', 'admin', '2024-04-18 00:34:02', '2024-04-18 00:34:02', 0),
('5815148aefc94cd6b4ae137e6e70d63e', 'Komiljon', 'EC59622B9B347DE4BD8DD210271F7D3B02485A9F572566131C92F0314D592587', 'muhammaduktamov22@gmail.com', 'admin', '2024-04-18 00:33:08', '2024-04-18 00:33:08', 0);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `users`
--
ALTER TABLE itransition_task4_iixn.users
  ADD PRIMARY KEY (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
