import React  from 'react';
import { Navigate } from 'react-router-dom';
import { Route, Routes, BrowserRouter as Router } from "react-router-dom";
import Login from './components/Login.js';
import PostCard from './components/Post/PostCard.js'
import ViewComment from './components/Comment/ViewComment.js';
import CommentCard from './components/Comment/CommentCard.js';
import ViewAllPost from './components/Post/ViewAllPost.js';
import Header from "./components/Header/Header.js";
import SideBar from "./components/SideBar/SideBar.js";
import MixPage from "./components/MixPage.js";
import Community from './components/Community/Community.js';
import Topic from "./components/Topic/Topic.js";
import Communities from "./components/Community/Communities.js";

function App() {
  let token = localStorage.getItem("Token");
  

  return (
        <Router>
          <Routes>
            <Route exact path="/" element={<Login/>} /> 
            <Route path="/login" element={<Login/>} />
            <Route path="/MixPage" element={<MixPage/>} /> 
            <Route path="/ViewComment" element={<ViewComment/>} />
            <Route path="/Community" element={<Community/>} />
            <Route path="/Topic" element={<Topic/>} />
            <Route path="/Communities" element={<Communities/>} />
          </Routes>
          

        </Router> 
  );
}

export default App;

