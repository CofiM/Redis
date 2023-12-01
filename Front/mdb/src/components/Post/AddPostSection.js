import { MDBCard, MDBCardTitle, MDBInput } from "mdb-react-ui-kit";
import Button from 'react-bootstrap/Button';
import ButtonGroup from 'react-bootstrap/ButtonGroup';
import Dropdown from 'react-bootstrap/Dropdown';
import DropdownItem from "react-bootstrap/esm/DropdownItem";
import DropdownMenu from "react-bootstrap/esm/DropdownMenu";
import React from "react";
import style from './Post.module.css';
import { useState,useEffect } from "react";


export default function AddPostSection(props)
{
    //
    const [topics,setTopics] = useState([]);
    const [topicInput,setTopic] = useState('---');
    const [titleInput,setTitle] = useState('');
    const [textInput,setText]= useState('');

    const token = localStorage.getItem("Token");

    const handleTopic = (event) => {setTopic(event.target.value);console.log(topicInput);};
    const handleTitle = (event) => {setTitle(event.target.value);console.log(titleInput);};
    const handleText = (event) => {setText(event.target.value);console.log(textInput);};


    const change = (eventkey) => {
        // a.persist();
        setTopic(eventkey)
        console.log(topicInput);
      };
    const submitHandler = () =>{
        console.log(props);
        const response = fetch("https://localhost:44368/Post/AddPost/" + titleInput + "/" + textInput + "/" + topicInput + "/" + props.userId + "/" + props.communityId,{
            method:'POST',
            headers: {
                'Content-type': 'application/json;charset=UTF-8',
                'Authorization': `Bearer ${token}`
            }
        });
        console.log(response.ok);
        window.location.reload(false);
    };

    const fetchData = async () =>
    {
        const response = await fetch("https://localhost:44368/Topic/GetAllTopic",
        {
            method: 'GET',
            headers:{'Content-type' : 'application/json; charset=UTF-8','Authorization': `Bearer ${token}`}
        });

        console.log(response);
        const data = await response.json();
        console.log(data);
        setTopics(data);
    };
    
    useEffect(()=>{
        fetchData();
    },[]);
    

    return(
        <div>
            <MDBCard className={style.AddPostDiv}>
            <div className={style.horizontalLine}>
                <label>Create post</label>
            </div>
         
            <div>
                <div className={style.groupDiv}>
                <span class="input-group-text border-0">Add topic</span>
                <input onChange={handleTopic}  value = {topicInput} type="text" class="form-control rounded"/>
                <Dropdown as={ButtonGroup} onSelect={change}>
                    <Button variant="primary">Topics</Button>
                    <Dropdown.Toggle split variant="primary" id="dropdown-split-basic" />
                    <Dropdown.Menu>
                        {topics.map((p)=>(<DropdownItem eventKey={p.name}>{p.name}</DropdownItem>))}
                        <Dropdown.Item eventKey="---">---</Dropdown.Item>
                    </Dropdown.Menu>
                 </Dropdown>
                </div>
                <div className={style.groupDiv}>
                    <span class="input-group-text border-0">Add title</span>
                    <input onChange={handleTitle} type="text" class="form-control rounded"/>
                </div>
                <div className={style.groupDiv}>
                    <span class="input-group-text border-0">Add text</span>
                    <textarea onChange={handleText} class="form-control rounded" aria-label="With textarea"></textarea>
                </div>
                <div className={style.centerBtn}>
                <Button variant="primary" onClick={submitHandler}>Post</Button>
                </div>
            </div>    
            </MDBCard>
        </div>
    );
}