import React, { Component } from 'react';
import axios from 'axios';
import ListItem from '../../components/ListItem';

class StudentList extends Component {
    state = {
        students: [],
        loading: true,
        error: ''
    };

    componentDidMount() {
        this.fetchStudents();
    }

    fetchStudents = () => {
        axios
            .get('https://localhost:44330/api/students')
            .then(res =>  this.setState({ students: res.data.students, loading: false, error: '' }))
            .catch(err => this.setState({ error: err.message, loading: false }))
    };

    render() {
        const { students } = this.state;

        return (
            <div className="student-list-container">
                <div>
                    {students.map(student => <ListItem key={student.studentId} value={student.studentName} />)}
                </div>
            </div>
        );
    }
}

export default StudentList;
