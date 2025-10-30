import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import type { SimpleTask } from '../models';
import constants from '../src/constants';

export interface TasksReducerState {
  get: {
    data: SimpleTask[];
    loading: boolean;
    error: string;
  };
  create: {
    data?: SimpleTask;
    loading: boolean;
    error: string;
  };
}

const initialState: TasksReducerState = {
  get: {
    data: [],
    loading: false,
    error: '',
  },
  create: {
    loading: false,
    error: '',
  },
};

export const fetchTasks = createAsyncThunk('tasks/fetchTasks', async () => {
  const response = await fetch(constants.getTasksEndpoint);
  return await response.json();
});

export const createTask = createAsyncThunk(
  'tasks/createTask',
  async (data: { title: string; description: string }): Promise<SimpleTask> => {
    const { title, description } = data;
    const options = {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        title,
        description,
      }),
    };

    const response = await fetch(constants.createTasksEndpoint, options);
    return (await response.json()) as SimpleTask;
  },
);

const tasksReducerSlice = createSlice({
  name: 'tasks',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(createTask.pending, (state) => {
        state.create.loading = true;
        state.create.error = '';
      })
      .addCase(createTask.fulfilled, (state, action) => {
        state.create.loading = false;
        state.create.data = action.payload;
        state.get.data.push(action.payload);
      })
      .addCase(createTask.rejected, (state, action) => {
        state.create.loading = false;
        state.create.error = action.error.message!;
      })
      .addCase(fetchTasks.pending, (state) => {
        state.get.loading = true;
        state.get.error = '';
      })
      .addCase(fetchTasks.fulfilled, (state, action) => {
        state.get.loading = false;
        state.get.data = action.payload;
      })
      .addCase(fetchTasks.rejected, (state, action) => {
        state.get.loading = false;
        state.get.error = action.error.message!;
      });
  },
});

export default tasksReducerSlice.reducer;
