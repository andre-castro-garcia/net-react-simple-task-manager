import { type ChangeEvent, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import type { AppDispatch, RootState } from '../store/store.ts';
import { createTask } from '../store/tasksReducer.ts';

export function CreateTask() {
  const dispatch = useDispatch<AppDispatch>();
  const [title, setTitle] = useState<string>();
  const [description, setDescription] = useState<string>();

  const { create } = useSelector((s: RootState) => s.tasks);
  const { data } = create;

  const handleTitleChange = (e: ChangeEvent<HTMLInputElement>) => {
    setTitle(e.target.value);
  };
  const handleDescriptionChange = (e: ChangeEvent<HTMLInputElement>) => {
    setDescription(e.target.value);
  };

  const handleClick = () => {
    if (title && description) {
      dispatch(createTask({ title, description }));
    }
  };

  useEffect(() => {
    if (data) {
      setTitle('');
      setDescription('');
    }
  }, [data]);

  return (
    <div>
      <div>
        <label htmlFor="title">Title</label>
        <input
          id="title"
          onChange={handleTitleChange}
          value={title}
          placeholder="Enter title here"
        />
      </div>
      <div>
        <label htmlFor="description">Description</label>
        <input
          id="title"
          onChange={handleDescriptionChange}
          value={description}
          placeholder="Enter description here"
        />
      </div>
      <div>
        <button disabled={!title || !description} onClick={() => handleClick()}>
          Create task
        </button>
      </div>
    </div>
  );
}
