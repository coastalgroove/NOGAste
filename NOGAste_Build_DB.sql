CREATE DATABASE  IF NOT EXISTS `securitylogs`;
USE `securitylogs`;

DROP TABLE IF EXISTS `events`;
CREATE TABLE `events` (
  `KeyID`        int(11)     NOT NULL AUTO_INCREMENT,
  `EventID`      int(11)     DEFAULT NULL,
  `EventMsg`     varchar(50) DEFAULT NULL,
  `TimeCreated`  varchar(50) DEFAULT NULL,
  `LogonType`    varchar(50) DEFAULT NULL,
  `LogonFail`    boolean NOT NULL DEFAULT 0,
  `AfterHours`   boolean NOT NULL DEFAULT 0,
  `LogonSuccess` boolean NOT NULL DEFAULT 0,
  `MachineName`  varchar(50) DEFAULT NULL,
  `UserID`       varchar(50) DEFAULT NULL,
  `ProgramRun`   varchar(50) DEFAULT NULL,
  `FileAccess`   varchar(50) DEFAULT NULL,  
  `LogLvl`       varchar(50) DEFAULT NULL,
  `Threat`       boolean NOT NULL DEFAULT 0,

  PRIMARY KEY (`KeyID`),
  UNIQUE KEY `KeyID_UNIQUE` (`KeyID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

INSERT INTO securityLogs.events (EventID, MachineName, UserName, LogLvl) VALUES (4624, "LingHo","Thatcher","WARNING");
INSERT INTO securityLogs.events (EventID, MachineName, UserName, LogLvl) VALUES (4624, "LingHo","user1",    "WARNING");
INSERT INTO securityLogs.events (EventID, MachineName, UserName, LogLvl) VALUES (4624, "LingHo","user2",    "WARNING");
INSERT INTO securityLogs.events (EventID, MachineName, UserName, LogLvl) VALUES (4624, "LingHo","JamesBond","WARNING");