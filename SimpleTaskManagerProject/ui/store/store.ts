import { configureStore } from '@reduxjs/toolkit';
import tasksReducerSlice from './tasksReducer';

export const store = configureStore({
  reducer: {
    tasks: tasksReducerSlice,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
