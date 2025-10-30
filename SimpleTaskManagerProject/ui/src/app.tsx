import { useEffect, useState } from 'react';
import type { SimpleTask } from '../models';
import constants from './constants.ts';

function App() {
  const [data, setData] = useState<SimpleTask[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');

  useEffect(() => {
    (async () => {
      try {
        const response = await fetch(constants.getTasksEndpoint);
        if (!response.ok) {
          setError('unknown error');
        }
        const jsonData = await response.json();
        setData(jsonData);
      } catch {
        setError('unknown error');
      } finally {
        setLoading(false);
      }
    })();
  }, []);

  return (
    <>
      {error ? (
        <div>{error}</div>
      ) : loading ? (
        <div>Loading...</div>
      ) : (
        <div>
          <table>
            <thead>
              <tr>
                <th>Task ID</th>
                <th>Title</th>
                <th>Description</th>
              </tr>
              {data.map((task) => (
                <tr>
                  <td>{task.id}</td>
                  <td>{task.title}</td>
                  <td>{task.description}</td>
                </tr>
              ))}
            </thead>
          </table>
        </div>
      )}
    </>
  );
}

export default App;
