import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import type { AppDispatch, RootState } from '../store/store.ts';
import { fetchTasks, addTask } from '../store/tasksReducer.ts';
import { CreateTask } from '../components/create-task.tsx';
import * as signalR from '@microsoft/signalr';
import constants from './constants.ts';
import type { SimpleTask } from '../models';

function App() {
  const [connection, setConnection] = useState<signalR.HubConnection>();

  const dispatch = useDispatch<AppDispatch>();
  const { get } = useSelector((s: RootState) => s.tasks);
  const { loading, error, data } = get;

  useEffect(() => {
    dispatch(fetchTasks());
  }, [dispatch]);

  useEffect(() => {
    const conn = new signalR.HubConnectionBuilder()
      .withUrl(constants.tasksHubEndpoint, { withCredentials: false })
      .withAutomaticReconnect()
      .build();
    setConnection(conn);
  }, []);

  useEffect(() => {
    if (connection) {
      connection.start().then(() => {
        connection.on('taskCreated', (data) => {
          const task = data as SimpleTask;
          dispatch(addTask(task));
        });
      });
    }
  }, [dispatch, connection]);

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
                <tr key={task.id}>
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
