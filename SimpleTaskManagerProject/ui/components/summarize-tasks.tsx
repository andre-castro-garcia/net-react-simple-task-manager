import { useDispatch, useSelector } from 'react-redux';
import type { AppDispatch, RootState } from '../store/store.ts';
import { summarizeTasks } from '../store/summarizeTasksReducer';

export function SummarizeTask() {
  const dispatch = useDispatch<AppDispatch>();

  const { data, loading } = useSelector((s: RootState) => s.summarizeTasks);

  const handleClick = () => {
    dispatch(summarizeTasks());
  };

  return (
    <div>
      <div>
        <button onClick={() => handleClick()}>Summarize Tasks</button>
      </div>
      {loading && <div>Loading...</div>}
      {data && <textarea>{data}</textarea>}
    </div>
  );
}
