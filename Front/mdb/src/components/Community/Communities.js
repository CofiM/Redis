import AllCommunities from "./AllCommunities.js";
import Header from "./HeaderCommunities.js";
import {useState, useEffect} from 'react';
import React from 'react';

export default function App(props){
    const [communities, setCommunities] = useState([]);

    async function fetchCommunities(){
        const response = await fetch("https://localhost:44368/GetAllComunities",
        {
            method: 'GET',
            headers: {
                'Content-type': 'application/json;charset=UTF-8'
            }
        });
        const data = await response.json();    
        
        console.log(data);
        
        setCommunities(data);
    }

    useEffect(()=>{
        fetchCommunities();
    },[]);

    return (
        <div>
            <Header />
            <div>
                {communities.map(item => (
                    <AllCommunities Name={item.name} ID={item.id}/>
                ))}
            </div>
        </div>
        
    );
}