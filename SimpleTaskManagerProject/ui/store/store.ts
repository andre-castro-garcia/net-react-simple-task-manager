import { configureStore } from '@reduxjs/toolkit';
import tasksReducerSlice from './tasksReducer';
import summarizeTasksReducerSlice from './summarizeTasksReducer';

export const store = configureStore({
  reducer: {
    tasks: tasksReducerSlice,
    summarizeTasks: summarizeTasksReducerSlice,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
