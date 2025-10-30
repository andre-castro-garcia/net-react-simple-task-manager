import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import type { AppDispatch, RootState } from '../store/store.ts';
import { fetchTasks } from '../store/tasksReducer.ts';
import { CreateTask } from '../components/create-task.tsx';

function App() {
  const dispatch = useDispatch<AppDispatch>();
  const { get } = useSelector((s: RootState) => s.tasks);
  const { loading, error, data } = get;

  useEffect(() => {
    dispatch(fetchTasks());
  }, [dispatch]);

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
      <div>
        <CreateTask />
      </div>
    </>
  );
}

export default App;
