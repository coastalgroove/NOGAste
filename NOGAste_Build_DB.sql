CREATE DATABASE  IF NOT EXISTS `securitylogs`;
USE `securitylogs`;

DROP TABLE IF EXISTS `events`;
CREATE TABLE `events` (
  `KeyID`        int(11)     NOT NULL AUTO_INCREMENT,
  `EventID`      varchar(10)  DEFAULT NULL,
  `TimeCreated`  varchar(25) DEFAULT NULL,
  `EventMsg`     varchar(100) DEFAULT NULL,
  `LogonType`    varchar(50) DEFAULT NULL,
  `LogonFail`    boolean NOT NULL DEFAULT 0,
  `FailInfo`     varchar(250) DEFAULT NULL,
  `FailReason`   varchar(250) DEFAULT NULL,
  `AfterHours`   boolean NOT NULL DEFAULT 0,
  `LogonSuccess` boolean NOT NULL DEFAULT 0,
  `MachineName`  varchar(50) DEFAULT NULL,
  `UserID`       varchar(50) DEFAULT NULL,
  `ProgramRun`   varchar(250) DEFAULT NULL,
  `CommandRun`   varchar(250) DEFAULT NULL,
  `FileAccess`   varchar(250) DEFAULT NULL,  
  `LogLvl`       varchar(50) DEFAULT NULL,
  `Status`       varchar(50) DEFAULT NULL,
  `SubStatus`    varchar(50) DEFAULT NULL,
  `Reason`       varchar(250) DEFAULT NULL,
  `Threat`       boolean NOT NULL DEFAULT 0,

  PRIMARY KEY (`KeyID`),
  UNIQUE KEY `KeyID_UNIQUE` (`KeyID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

INSERT INTO securityLogs.events (EventID, TimeCreated, MachineName, UserID, LogLvl) VALUES (4624, '2023-08-01 14:01:30',  "LingHo","ChuckNorris","WARNING");
INSERT INTO securityLogs.events (EventID, TimeCreated, MachineName, UserID, LogLvl) VALUES (4624, '2023-08-01 15:21:40',  "LingHo","user1",    "WARNING");
INSERT INTO securityLogs.events (EventID, TimeCreated, MachineName, UserID, LogLvl) VALUES (4624, '2023-08-01 16:34:43',  "LingHo","user2",    "WARNING");
INSERT INTO securityLogs.events (EventID, TimeCreated, MachineName, UserID, LogLvl) VALUES (4624, '2023-08-01 17:55:12',  "LingHo","JamesBond","WARNING");