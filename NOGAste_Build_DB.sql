CREATE DATABASE  IF NOT EXISTS `securitylogs`;
USE `securitylogs`;

DROP TABLE IF EXISTS `events`;
CREATE TABLE `events` (
  `KeyID`          int(11)     NOT NULL AUTO_INCREMENT,
  `EventID`        varchar(10)  DEFAULT NULL,
  `TimeCreated`    varchar(25) DEFAULT NULL,
  `EventMsg`       varchar(250) DEFAULT NULL,
  `LogonType`      varchar(50) DEFAULT NULL,
  `ElevToken`      varchar(50) DEFAULT NULL,
  `ImpersonateLvl` varchar(50) DEFAULT NULL,
  `LogonFail`      varchar(50) DEFAULT NULL,
  `KnownUser`      varchar(250) DEFAULT NULL,
  `FailReason`     varchar(500) DEFAULT NULL,
  `AfterHours`     varchar(50) DEFAULT NULL,
  `LogonSuccess`   varchar(50) DEFAULT NULL,
  `MachineName`    varchar(50) DEFAULT NULL,
  `UserID`         varchar(50) DEFAULT NULL,
  `ProgramRun`     varchar(250) DEFAULT NULL,
  `CommandRun`     varchar(250) DEFAULT NULL,
  `ProcessInfo`    varchar(250) DEFAULT NULL,  
  `ObjName`        varchar(250) DEFAULT NULL, 
  `AppPath`        varchar(250) DEFAULT NULL, 
  `LogLvl`         varchar(50) DEFAULT NULL,
  `Status`         varchar(50) DEFAULT NULL,
  `SubStatus`      varchar(50) DEFAULT NULL,
  `ReasonEvnt`     varchar(250) DEFAULT NULL,
  `ThreatEval`     varchar(10) DEFAULT NULL,
  `ActionReqd`     varchar(250) DEFAULT NULL,

  PRIMARY KEY (`KeyID`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

INSERT INTO securityLogs.events (EventID, TimeCreated, MachineName, UserID, LogLvl, KnownUser, ThreatEval, ActionReqd ) VALUES (4624, '2023-08-01 14:01:30',  "LingHo","ChuckNorris","WARNING", "KNOWN", "TBD", "TBD" );
INSERT INTO securityLogs.events (EventID, TimeCreated, MachineName, UserID, LogLvl, KnownUser, ThreatEval, ActionReqd ) VALUES (4624, '2023-08-01 15:21:40',  "LingHo","user1",      "WARNING", "KNOWN", "TBD", "TBD" );
INSERT INTO securityLogs.events (EventID, TimeCreated, MachineName, UserID, LogLvl, KnownUser, ThreatEval, ActionReqd ) VALUES (4624, '2023-08-01 16:34:43',  "LingHo","user2",      "WARNING", "KNOWN", "TBD", "TBD" );
INSERT INTO securityLogs.events (EventID, TimeCreated, MachineName, UserID, LogLvl, KnownUser, ThreatEval, ActionReqd ) VALUES (4624, '2023-08-01 17:55:12',  "LingHo","JamesBond",  "WARNING", "KNOWN", "TBD", "TBD" );