import { useEffect, useState } from "react";
import './App.css'

interface WeatherForecast {
    Id: number;
}

function App() {
    const [data, setData] = useState<WeatherForecast[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string>('');
    
    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch('http://localhost:5218/weatherforecast');
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                const jsonData = await response.json();
                setData(jsonData);
            } catch {
                setError('unknown error');
            } finally {
                setLoading(false);
            }
        };
        fetchData();
    }, []);
    
  return (
    <>
      <div>{data.length}</div>
    </>
  )
}

export default App
