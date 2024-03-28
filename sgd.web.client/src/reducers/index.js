// src/reducers/index.js
import { combineReducers } from 'redux';

const keyToken = process.env.REACT_APP_LS_KEY_SGDWEBCLIENTJWT;

// Reducers
const initialState = {
  estaLogueado: localStorage[keyToken] != null
};

const appReducer = (state = initialState, action) => {
  switch (action.type) {
    case 'SET_ESTALOGUEADO':
      return { ...state, estaLogueado: action.payload };
    default:
      return state;
  }
};

// Combine Reducers
const rootReducer = combineReducers({app: appReducer});

export default rootReducer;