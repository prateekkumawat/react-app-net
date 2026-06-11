import { useEffect, useState } from 'react';

export default function App() {
  const apiBaseUrl = import.meta.env.VITE_API_BASE_URL || '';
  const [health, setHealth] = useState('Loading...');
  const [dbStatus, setDbStatus] = useState('Checking database connection...');

  useEffect(() => {
    const baseUrl = apiBaseUrl.replace(/\/$/, '');

    fetch(`${baseUrl}/api/health`)
      .then((res) => res.json())
      .then((data) => setHealth(data.message || 'Backend is running.'))
      .catch(() => setHealth('Backend is not running yet. Start the .NET API on port 8080.'));

    fetch(`${baseUrl}/api/db-check`)
      .then(async (res) => {
        const data = await res.json();
        if (!res.ok) {
          setDbStatus(data.message || 'Database check failed.');
          return;
        }
        setDbStatus(`Connected to ${data.database || 'SQL Server'} on ${data.server || 'configured server'}.`);
      })
      .catch(() => setDbStatus('Database connectivity check unavailable.')); 
  }, []);

  return (
    <main className="app-shell">
      <section className="card">
        <p className="eyebrow">React + .NET Azure Sample</p>
        <h1>Frontend and backend are connected through Azure-friendly APIs.</h1>
        <p className="lead">This sample uses a Vite React client and a minimal .NET Web API with SQL Server connectivity checks.</p>

        <div className="grid">
          <article className="panel">
            <h2>API Health</h2>
            <p>{health}</p>
          </article>
          <article className="panel">
            <h2>Database Status</h2>
            <p>{dbStatus}</p>
          </article>
        </div>
      </section>
    </main>
  );
}
