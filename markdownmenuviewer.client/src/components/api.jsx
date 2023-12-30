import { useEffect, useState } from "react";

export default function Api() {
  const [countries, setCountries] = useState([]);

  useEffect(() => {
    fetch("https://restcountries.com/v3.1/all")
      .then((response) => response.json())
      .then((veri) => setCountries(veri));
  }, []);

  return (
    <div>
      <h1>Ülkeler</h1>
      <h3>Api dan veri al</h3>
      {countries.map((ulke) => {
        return (
          <div key={ulke.name.common}>
            <h2>{ulke.name.common}</h2>
            <h4>{ulke.capital}</h4>
            <img
              src={ulke.flags.png}
              alt={ulke.name.common}
              style={{ width: "150px" }}
            />
          </div>
        );
      })}
    </div>
  );
}
