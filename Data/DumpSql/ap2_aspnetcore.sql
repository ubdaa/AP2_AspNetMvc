-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 109.199.107.134:3306
-- Generation Time: Nov 22, 2024 at 10:03 PM
-- Server version: 8.4.3
-- PHP Version: 8.3.13

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `ap2_aspnetcore`
--

-- --------------------------------------------------------

--
-- Table structure for table `Allergies`
--

CREATE TABLE `Allergies` (
  `AllergyId` int NOT NULL,
  `Name` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `Allergies`
--

INSERT INTO `Allergies` (`AllergyId`, `Name`) VALUES
(1, 'Pollen'),
(2, 'Poussière'),
(3, 'Arachides'),
(4, 'Noix d’arbre'),
(5, 'Lait'),
(6, 'Œufs'),
(7, 'Fruits de mer'),
(8, 'Blé'),
(9, 'Soja'),
(10, 'Poisson'),
(11, 'Moisissures'),
(12, 'Piqûres d’insectes'),
(13, 'Latex'),
(14, 'Poils d’animaux'),
(15, 'Pollen de graminées'),
(16, 'Ambroisie'),
(17, 'Venin d’abeille'),
(18, 'Pénicilline'),
(19, 'Médicaments sulfonamides'),
(20, 'Aspirine'),
(21, 'Parfum'),
(22, 'Nickel'),
(23, 'Gluten'),
(24, 'Caféine'),
(25, 'Chocolat'),
(26, 'Lumière du soleil'),
(27, 'Froid'),
(28, 'Chlore'),
(29, 'Teinture pour cheveux'),
(30, 'Cosmétiques'),
(31, 'Fragrance'),
(32, 'Assouplissant'),
(33, 'Détergent'),
(34, 'Colorant rouge'),
(35, 'Édulcorants artificiels'),
(36, 'Maïs'),
(37, 'Glutamate monosodique (MSG)'),
(38, 'Alcool'),
(39, 'Ail'),
(40, 'Oignon'),
(41, 'Avocat'),
(42, 'Tomate'),
(43, 'Banane'),
(44, 'Kiwi'),
(45, 'Papaye'),
(46, 'Ananas'),
(47, 'Graines de sésame'),
(48, 'Moutarde'),
(49, 'Orge'),
(50, 'Avoine'),
(51, 'Céleri');

-- --------------------------------------------------------

--
-- Table structure for table `AllergyMedicament`
--

CREATE TABLE `AllergyMedicament` (
  `AllergiesAllergyId` int NOT NULL,
  `MedicamentsMedicamentId` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `AllergyPatient`
--

CREATE TABLE `AllergyPatient` (
  `AllergiesAllergyId` int NOT NULL,
  `PatientsPatientId` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `AllergyPatient`
--

INSERT INTO `AllergyPatient` (`AllergiesAllergyId`, `PatientsPatientId`) VALUES
(5, 1),
(7, 1),
(9, 1),
(11, 1),
(3, 2),
(5, 2),
(9, 2);

-- --------------------------------------------------------

--
-- Table structure for table `AspNetRoleClaims`
--

CREATE TABLE `AspNetRoleClaims` (
  `Id` int NOT NULL,
  `RoleId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `AspNetRoles`
--

CREATE TABLE `AspNetRoles` (
  `Id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `AspNetUserClaims`
--

CREATE TABLE `AspNetUserClaims` (
  `Id` int NOT NULL,
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `AspNetUserLogins`
--

CREATE TABLE `AspNetUserLogins` (
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderKey` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderDisplayName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `AspNetUserRoles`
--

CREATE TABLE `AspNetUserRoles` (
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RoleId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `AspNetUsers`
--

CREATE TABLE `AspNetUsers` (
  `Id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `FirstName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LastName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Address` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Faculty` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Specialty` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `UserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Email` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `SecurityStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PhoneNumber` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `AspNetUsers`
--

INSERT INTO `AspNetUsers` (`Id`, `FirstName`, `LastName`, `Address`, `Faculty`, `Specialty`, `UserName`, `NormalizedUserName`, `Email`, `NormalizedEmail`, `EmailConfirmed`, `PasswordHash`, `SecurityStamp`, `ConcurrencyStamp`, `PhoneNumber`, `PhoneNumberConfirmed`, `TwoFactorEnabled`, `LockoutEnd`, `LockoutEnabled`, `AccessFailedCount`) VALUES
('382a2db4-d3c4-4314-83a9-14e3d82e9bbb', 'Abdurahmen', 'Gharsalli', '1 rue dioejdzodze', 'Faculté de médecine générale de Lyon', 'Généraliste', 'abdu', 'ABDU', 'abdurahmen.ghar@gmail.com', 'ABDURAHMEN.GHAR@GMAIL.COM', 0, 'AQAAAAIAAYagAAAAEDE+wWL9WgqP+MS3Xybf5EEBDxSRLzgQF+uOF+l/yHy7PLOZYqAj0WKJlDdpUVlk5A==', '6T53E5FDPPX3KVZ2Q72FUO2AMVXH5XLY', '6ac11c76-7f79-4da6-9d14-c18f95f941cd', NULL, 0, 0, NULL, 1, 0);

-- --------------------------------------------------------

--
-- Table structure for table `AspNetUserTokens`
--

CREATE TABLE `AspNetUserTokens` (
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `MedicalHistories`
--

CREATE TABLE `MedicalHistories` (
  `MedicalHistoryId` int NOT NULL,
  `Name` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `MedicalHistories`
--

INSERT INTO `MedicalHistories` (`MedicalHistoryId`, `Name`) VALUES
(1, 'Diabète de type 1'),
(2, 'Diabète de type 2'),
(3, 'Hypertension'),
(4, 'Insuffisance cardiaque'),
(5, 'Asthme'),
(6, 'Bronchite chronique'),
(7, 'Pneumonie'),
(8, 'Tuberculose'),
(9, 'Hépatite B'),
(10, 'Hépatite C'),
(11, 'SIDA/VIH'),
(12, 'Anémie'),
(13, 'Insuffisance rénale'),
(14, 'Calculs rénaux'),
(15, 'Cirrhose'),
(16, 'Ulcère gastrique'),
(17, 'Reflux gastro-œsophagien'),
(18, 'Migraines'),
(19, 'Épilepsie'),
(20, 'Sclérose en plaques'),
(21, 'Maladie de Parkinson'),
(22, 'Alzheimer'),
(23, 'Démence'),
(24, 'Dépression'),
(25, 'Anxiété'),
(26, 'Trouble bipolaire'),
(27, 'Schizophrénie'),
(28, 'Cancer du poumon'),
(29, 'Cancer du sein'),
(30, 'Cancer de la prostate'),
(31, 'Cancer colorectal'),
(32, 'Cancer de la peau'),
(33, 'Leucémie'),
(34, 'Lupus'),
(35, 'Polyarthrite rhumatoïde'),
(36, 'Ostéoporose'),
(37, 'Hypothyroïdie'),
(38, 'Hyperthyroïdie'),
(39, 'Arthrose'),
(40, 'Infarctus du myocarde'),
(41, 'Accident vasculaire cérébral'),
(42, 'Obésité'),
(43, 'Hypercholestérolémie'),
(44, 'Maladie de Crohn'),
(45, 'Rectocolite hémorragique'),
(46, 'Syndrome de l’intestin irritable'),
(47, 'Endométriose'),
(48, 'Infertilité'),
(49, 'Problèmes auditifs'),
(50, 'Troubles de la vision'),
(51, 'Maladie d’Alzheimer précoce');

-- --------------------------------------------------------

--
-- Table structure for table `MedicalHistoryMedicament`
--

CREATE TABLE `MedicalHistoryMedicament` (
  `MedicalHistoriesMedicalHistoryId` int NOT NULL,
  `MedicamentsMedicamentId` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `MedicalHistoryPatient`
--

CREATE TABLE `MedicalHistoryPatient` (
  `MedicalHistoriesMedicalHistoryId` int NOT NULL,
  `PatientsPatientId` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `MedicalHistoryPatient`
--

INSERT INTO `MedicalHistoryPatient` (`MedicalHistoriesMedicalHistoryId`, `PatientsPatientId`) VALUES
(2, 1),
(7, 1),
(8, 1),
(3, 2),
(5, 2),
(7, 2);

-- --------------------------------------------------------

--
-- Table structure for table `MedicamentPrescription`
--

CREATE TABLE `MedicamentPrescription` (
  `MedicamentsMedicamentId` int NOT NULL,
  `PrescriptionsPrescriptionId` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `MedicamentPrescription`
--

INSERT INTO `MedicamentPrescription` (`MedicamentsMedicamentId`, `PrescriptionsPrescriptionId`) VALUES
(9, 1),
(17, 1),
(28, 1),
(1, 3),
(6, 3),
(13, 3);

-- --------------------------------------------------------

--
-- Table structure for table `Medicaments`
--

CREATE TABLE `Medicaments` (
  `MedicamentId` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `Name` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Quantity` varchar(1024) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Ingredients` varchar(1024) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Type` int NOT NULL,
  `Category` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `Medicaments`
--

INSERT INTO `Medicaments` (`MedicamentId`, `CreatedAt`, `Name`, `Quantity`, `Ingredients`, `Type`, `Category`) VALUES
(1, '2024-11-19 08:33:47.000000', 'Paracétamol', '500mg', 'Paracetamol, Acide stéarique, Povidone', 0, 1),
(2, '2024-11-19 08:33:47.000000', 'Amoxicilline', '250mg', 'Amoxicillin, Cellulose, Stéarate de magnésium', 1, 1),
(3, '2024-11-19 08:33:47.000000', 'Crème antifongique', '30g', 'Clotrimazole, Paraffine, Alcool benzylique', 2, 2),
(4, '2024-11-19 08:33:47.000000', 'Ibuprofène', '400mg', 'Ibuprofen, Lactose monohydraté, Silice colloïdale', 0, 7),
(5, '2024-11-19 08:33:47.000000', 'Vaccin COVID-19', '1 dose', 'ARNm, Lipides, Chlorure de potassium', 4, 34),
(6, '2024-11-19 08:33:47.000000', 'Suppositoire de glycérine', '1g', 'Glycérine, Acide stéarique, Eau purifiée', 5, 23),
(7, '2024-11-19 08:33:47.000000', 'Pommade à l’hydrocortisone', '15g', 'Hydrocortisone, Vaseline, Paraffine liquide', 6, 17),
(8, '2024-11-19 08:33:47.000000', 'Collyre antiallergique', '10ml', 'Olopatadine, Chlorure de sodium, Acide borique', 10, 6),
(9, '2024-11-19 08:33:47.000000', 'Patch de nicotine', '21mg', 'Nicotine, Cellulose, Polyéthylène', 9, 31),
(10, '2024-11-19 08:33:47.000000', 'Inhalateur de salbutamol', '100mcg', 'Salbutamol, Norflurane, Alcool anhydre', 8, 14),
(11, '2024-11-19 08:33:47.000000', 'Aspirine', '100mg', 'Acide acétylsalicylique, Amidon de maïs, Talc', 0, 1),
(12, '2024-11-19 08:33:47.000000', 'Crème hydratante avec urée', '50g', 'Urée, Glycérine, Alcool cétylique', 2, 17),
(13, '2024-11-19 08:33:47.000000', 'Sirop contre la toux', '200ml', 'Dextrométhorphane, Sorbitol, Acide citrique', 1, 1),
(14, '2024-11-19 08:33:47.000000', 'Gélule de vitamine C', '500mg', 'Acide ascorbique, Gélatine, Stéarate de magnésium', 3, 36),
(15, '2024-11-19 08:33:47.000000', 'Solution saline nasale', '15ml', 'Chlorure de sodium, Eau purifiée, Chlorure de benzalkonium', 10, 30),
(16, '2024-11-19 08:33:47.000000', 'Suppositoire de paracétamol', '250mg', 'Paracetamol, Glycérine, Acide citrique', 5, 1),
(17, '2024-11-19 08:33:47.000000', 'Gel antibactérien', '50g', 'Chlorhexidine, Eau purifiée, Alcool isopropylique', 6, 2),
(18, '2024-11-19 08:33:47.000000', 'Antiémétique oral', '8mg', 'Ondansétron, Lactose, Cellulose', 0, 10),
(19, '2024-11-19 08:33:47.000000', 'Crème pour l’acné', '30g', 'Peroxyde de benzoyle, Glycérine, Acide stéarique', 2, 17),
(20, '2024-11-19 08:33:47.000000', 'Capsules d’oméga-3', '1000mg', 'Huile de poisson, Gélatine, Glycérol', 3, 31),
(21, '2024-11-19 08:33:47.000000', 'Sirop antitussif', '125ml', 'Guaifénésine, Sorbitol, Acide citrique', 1, 8),
(22, '2024-11-19 08:33:47.000000', 'Inhalateur de corticostéroïde', '200mcg', 'Fluticasone, Norflurane, Ethanol', 8, 17),
(23, '2024-11-19 08:33:47.000000', 'Solution pour lentilles', '100ml', 'Chlorure de sodium, Acide borique, Eau purifiée', 10, 29),
(24, '2024-11-19 08:33:47.000000', 'Antidouleur opiacé', '50mg', 'Tramadol, Lactose, Stéarate de magnésium', 0, 1),
(25, '2024-11-19 08:33:47.000000', 'Pommade antibactérienne', '20g', 'Néomycine, Vaseline, Lanoline', 6, 2),
(26, '2024-11-19 08:33:47.000000', 'Crème antivirale', '10g', 'Acyclovir, Paraffine, Alcool benzylique', 2, 3),
(27, '2024-11-19 08:33:47.000000', 'Pilule contraceptive', '1mg', 'Éthinylestradiol, Lévonorgestrel, Lactose', 0, 44),
(28, '2024-11-19 08:33:47.000000', 'Gouttes nasales', '10ml', 'Xylométazoline, Chlorure de sodium, Eau purifiée', 10, 30),
(29, '2024-11-19 08:33:47.000000', 'Injection d’insuline', '10ml', 'Insuline, Phosphate de sodium, Glycérol', 4, 5),
(30, '2024-11-19 08:33:47.000000', 'Spray nasal antihistaminique', '15ml', 'Azelastine, Chlorure de sodium, Acide citrique', 10, 6),
(31, '2024-11-19 08:33:47.000000', 'Sérum physiologique', '500ml', 'Chlorure de sodium, Eau distillée, Bicarbonate de sodium', 10, 30);

-- --------------------------------------------------------

--
-- Table structure for table `Patients`
--

CREATE TABLE `Patients` (
  `PatientId` int NOT NULL,
  `FirstName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LastName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `BirthDate` date NOT NULL,
  `Age` int NOT NULL,
  `Gender` int NOT NULL,
  `Height` int NOT NULL,
  `Weight` int NOT NULL,
  `Address` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `SocialSecurityNumber` varchar(18) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `DoctorId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `Patients`
--

INSERT INTO `Patients` (`PatientId`, `FirstName`, `LastName`, `BirthDate`, `Age`, `Gender`, `Height`, `Weight`, `Address`, `SocialSecurityNumber`, `CreatedAt`, `DoctorId`) VALUES
(1, 'Nicolas', 'Pons', '1992-07-29', 32, 0, 180, 80, '1 rue vraie rue', '1 85 12 75 123 456', '2024-11-19 09:37:37.419785', '382a2db4-d3c4-4314-83a9-14e3d82e9bbb'),
(2, 'Safwane', 'Remadi', '2005-07-09', 19, 0, 180, 80, '1 rue malleval', '1 85 12 75 123 456', '2024-11-22 20:08:42.405915', '382a2db4-d3c4-4314-83a9-14e3d82e9bbb');

-- --------------------------------------------------------

--
-- Table structure for table `Prescriptions`
--

CREATE TABLE `Prescriptions` (
  `PrescriptionId` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `StartDate` date DEFAULT NULL,
  `EndDate` date DEFAULT NULL,
  `Dosage` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `AdditionalInformation` varchar(2048) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `PatientId` int NOT NULL,
  `DoctorId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `Prescriptions`
--

INSERT INTO `Prescriptions` (`PrescriptionId`, `CreatedAt`, `StartDate`, `EndDate`, `Dosage`, `AdditionalInformation`, `PatientId`, `DoctorId`) VALUES
(1, '2024-11-19 09:37:52.470969', '2024-11-12', '2024-11-28', '3 fois par jour', NULL, 1, '382a2db4-d3c4-4314-83a9-14e3d82e9bbb'),
(2, '2024-11-20 09:40:53.284467', '2024-11-20', '2024-11-22', NULL, NULL, 1, '382a2db4-d3c4-4314-83a9-14e3d82e9bbb'),
(3, '2024-11-22 20:52:34.328957', '2024-11-19', '2024-11-27', 'Tous les jours', 'Faire attention sur le sirop contre la toux', 2, '382a2db4-d3c4-4314-83a9-14e3d82e9bbb');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `Allergies`
--
ALTER TABLE `Allergies`
  ADD PRIMARY KEY (`AllergyId`);

--
-- Indexes for table `AllergyMedicament`
--
ALTER TABLE `AllergyMedicament`
  ADD PRIMARY KEY (`AllergiesAllergyId`,`MedicamentsMedicamentId`),
  ADD KEY `IX_AllergyMedicament_MedicamentsMedicamentId` (`MedicamentsMedicamentId`);

--
-- Indexes for table `AllergyPatient`
--
ALTER TABLE `AllergyPatient`
  ADD PRIMARY KEY (`AllergiesAllergyId`,`PatientsPatientId`),
  ADD KEY `IX_AllergyPatient_PatientsPatientId` (`PatientsPatientId`);

--
-- Indexes for table `AspNetRoleClaims`
--
ALTER TABLE `AspNetRoleClaims`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`);

--
-- Indexes for table `AspNetRoles`
--
ALTER TABLE `AspNetRoles`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `RoleNameIndex` (`NormalizedName`);

--
-- Indexes for table `AspNetUserClaims`
--
ALTER TABLE `AspNetUserClaims`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_AspNetUserClaims_UserId` (`UserId`);

--
-- Indexes for table `AspNetUserLogins`
--
ALTER TABLE `AspNetUserLogins`
  ADD PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  ADD KEY `IX_AspNetUserLogins_UserId` (`UserId`);

--
-- Indexes for table `AspNetUserRoles`
--
ALTER TABLE `AspNetUserRoles`
  ADD PRIMARY KEY (`UserId`,`RoleId`),
  ADD KEY `IX_AspNetUserRoles_RoleId` (`RoleId`);

--
-- Indexes for table `AspNetUsers`
--
ALTER TABLE `AspNetUsers`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  ADD KEY `EmailIndex` (`NormalizedEmail`);

--
-- Indexes for table `AspNetUserTokens`
--
ALTER TABLE `AspNetUserTokens`
  ADD PRIMARY KEY (`UserId`,`LoginProvider`,`Name`);

--
-- Indexes for table `MedicalHistories`
--
ALTER TABLE `MedicalHistories`
  ADD PRIMARY KEY (`MedicalHistoryId`);

--
-- Indexes for table `MedicalHistoryMedicament`
--
ALTER TABLE `MedicalHistoryMedicament`
  ADD PRIMARY KEY (`MedicalHistoriesMedicalHistoryId`,`MedicamentsMedicamentId`),
  ADD KEY `IX_MedicalHistoryMedicament_MedicamentsMedicamentId` (`MedicamentsMedicamentId`);

--
-- Indexes for table `MedicalHistoryPatient`
--
ALTER TABLE `MedicalHistoryPatient`
  ADD PRIMARY KEY (`MedicalHistoriesMedicalHistoryId`,`PatientsPatientId`),
  ADD KEY `IX_MedicalHistoryPatient_PatientsPatientId` (`PatientsPatientId`);

--
-- Indexes for table `MedicamentPrescription`
--
ALTER TABLE `MedicamentPrescription`
  ADD PRIMARY KEY (`MedicamentsMedicamentId`,`PrescriptionsPrescriptionId`),
  ADD KEY `IX_MedicamentPrescription_PrescriptionsPrescriptionId` (`PrescriptionsPrescriptionId`);

--
-- Indexes for table `Medicaments`
--
ALTER TABLE `Medicaments`
  ADD PRIMARY KEY (`MedicamentId`);

--
-- Indexes for table `Patients`
--
ALTER TABLE `Patients`
  ADD PRIMARY KEY (`PatientId`),
  ADD KEY `IX_Patients_DoctorId` (`DoctorId`);

--
-- Indexes for table `Prescriptions`
--
ALTER TABLE `Prescriptions`
  ADD PRIMARY KEY (`PrescriptionId`),
  ADD KEY `IX_Prescriptions_DoctorId` (`DoctorId`),
  ADD KEY `IX_Prescriptions_PatientId` (`PatientId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `Allergies`
--
ALTER TABLE `Allergies`
  MODIFY `AllergyId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=53;

--
-- AUTO_INCREMENT for table `AspNetRoleClaims`
--
ALTER TABLE `AspNetRoleClaims`
  MODIFY `Id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `AspNetUserClaims`
--
ALTER TABLE `AspNetUserClaims`
  MODIFY `Id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `MedicalHistories`
--
ALTER TABLE `MedicalHistories`
  MODIFY `MedicalHistoryId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=52;

--
-- AUTO_INCREMENT for table `Medicaments`
--
ALTER TABLE `Medicaments`
  MODIFY `MedicamentId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=32;

--
-- AUTO_INCREMENT for table `Patients`
--
ALTER TABLE `Patients`
  MODIFY `PatientId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `Prescriptions`
--
ALTER TABLE `Prescriptions`
  MODIFY `PrescriptionId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `AllergyMedicament`
--
ALTER TABLE `AllergyMedicament`
  ADD CONSTRAINT `FK_AllergyMedicament_Allergies_AllergiesAllergyId` FOREIGN KEY (`AllergiesAllergyId`) REFERENCES `Allergies` (`AllergyId`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_AllergyMedicament_Medicaments_MedicamentsMedicamentId` FOREIGN KEY (`MedicamentsMedicamentId`) REFERENCES `Medicaments` (`MedicamentId`) ON DELETE CASCADE;

--
-- Constraints for table `AllergyPatient`
--
ALTER TABLE `AllergyPatient`
  ADD CONSTRAINT `FK_AllergyPatient_Allergies_AllergiesAllergyId` FOREIGN KEY (`AllergiesAllergyId`) REFERENCES `Allergies` (`AllergyId`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_AllergyPatient_Patients_PatientsPatientId` FOREIGN KEY (`PatientsPatientId`) REFERENCES `Patients` (`PatientId`) ON DELETE CASCADE;

--
-- Constraints for table `AspNetRoleClaims`
--
ALTER TABLE `AspNetRoleClaims`
  ADD CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `AspNetUserClaims`
--
ALTER TABLE `AspNetUserClaims`
  ADD CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `AspNetUserLogins`
--
ALTER TABLE `AspNetUserLogins`
  ADD CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `AspNetUserRoles`
--
ALTER TABLE `AspNetUserRoles`
  ADD CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `AspNetUserTokens`
--
ALTER TABLE `AspNetUserTokens`
  ADD CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `MedicalHistoryMedicament`
--
ALTER TABLE `MedicalHistoryMedicament`
  ADD CONSTRAINT `FK_MedicalHistoryMedicament_MedicalHistories_MedicalHistoriesMe~` FOREIGN KEY (`MedicalHistoriesMedicalHistoryId`) REFERENCES `MedicalHistories` (`MedicalHistoryId`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_MedicalHistoryMedicament_Medicaments_MedicamentsMedicamentId` FOREIGN KEY (`MedicamentsMedicamentId`) REFERENCES `Medicaments` (`MedicamentId`) ON DELETE CASCADE;

--
-- Constraints for table `MedicalHistoryPatient`
--
ALTER TABLE `MedicalHistoryPatient`
  ADD CONSTRAINT `FK_MedicalHistoryPatient_MedicalHistories_MedicalHistoriesMedic~` FOREIGN KEY (`MedicalHistoriesMedicalHistoryId`) REFERENCES `MedicalHistories` (`MedicalHistoryId`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_MedicalHistoryPatient_Patients_PatientsPatientId` FOREIGN KEY (`PatientsPatientId`) REFERENCES `Patients` (`PatientId`) ON DELETE CASCADE;

--
-- Constraints for table `MedicamentPrescription`
--
ALTER TABLE `MedicamentPrescription`
  ADD CONSTRAINT `FK_MedicamentPrescription_Medicaments_MedicamentsMedicamentId` FOREIGN KEY (`MedicamentsMedicamentId`) REFERENCES `Medicaments` (`MedicamentId`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_MedicamentPrescription_Prescriptions_PrescriptionsPrescripti~` FOREIGN KEY (`PrescriptionsPrescriptionId`) REFERENCES `Prescriptions` (`PrescriptionId`) ON DELETE CASCADE;

--
-- Constraints for table `Patients`
--
ALTER TABLE `Patients`
  ADD CONSTRAINT `FK_Patients_AspNetUsers_DoctorId` FOREIGN KEY (`DoctorId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `Prescriptions`
--
ALTER TABLE `Prescriptions`
  ADD CONSTRAINT `FK_Prescriptions_AspNetUsers_DoctorId` FOREIGN KEY (`DoctorId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Prescriptions_Patients_PatientId` FOREIGN KEY (`PatientId`) REFERENCES `Patients` (`PatientId`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
