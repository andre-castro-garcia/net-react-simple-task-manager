import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import constants from '../src/constants';

export interface SummarizeTasksReducerState {
  data?: string;
  loading: boolean;
  error: string;
}

const initialState: SummarizeTasksReducerState = {
  loading: false,
  error: '',
};

export const summarizeTasks = createAsyncThunk('summarize-tasks/summarizeTasks', async () => {
  const response = await fetch(constants.summarizedTasksEndpoint);
  return await response.json();
});

const summarizeTasksReducerSlice = createSlice({
  name: 'summarize-tasks',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(summarizeTasks.pending, (state) => {
        state.loading = true;
        state.error = '';
      })
      .addCase(summarizeTasks.fulfilled, (state, action) => {
        state.loading = false;
        state.data = action.payload.summary;
      })
      .addCase(summarizeTasks.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message!;
      });
  },
});

export default summarizeTasksReducerSlice.reducer;
