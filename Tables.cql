CREATE TABLE solarpoc.weatherHourly ( 
  latitude int,
  longitude int,
  
  apparentTemperature float,
  cloudCover float,
  dewPoint float,
  humidity float,
  icon varchar,
  ozone float,
  precipIntensity float,
  precipProbability float,
  precipType varchar,
  pressure float,
  summary varchar,
  temperature float,
  time bigint,
  visibility float,
  windBearing float,
  windSpeed float,

  PRIMARY KEY ((latitude, longitude), time)
);
  
  
CREATE TABLE solarpoc.weatherDaily (
  latitude int,
  longitude int,
  
  apparentTemperatureMax float,
  apparentTemperatureMaxTime bigint,
  apparentTemperatureMin float,
  apparentTemperatureMinTime bigint,
  cloudCover float,
  dewPoint float,
  humidity float,
  icon varchar,
  moonPhase float,
  ozone float,
  precipIntensity float,
  precipIntensityMax float,
  precipProbability float,
  pressure float, 
  summary varchar,
  sunriseTime bigint,
  sunsetTime bigint,
  temperatureMax float,
  temperatureMaxTime bigint,
  temperatureMin float,
  temperatureMinTime bigint,
  time bigint,
  visibility float,
  windBearing float,
  windSpeed float,
  
  PRIMARY KEY ((latitude, longitude), time)
);
  