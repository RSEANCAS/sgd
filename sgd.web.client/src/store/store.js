// src/store/store.js
import { configureStore } from '@reduxjs/toolkit';
import appReducer from '../reducers/index'; 

const store = configureStore({
  reducer: appReducer,
});

export default store;