import React, { useEffect, useState } from 'react';

function App() {
  const [customers, setCustomers] = useState([]);

  useEffect(() => {
    // execute this code when the page is loaded
    fetch('http://localhost:3100/api/customers')
      .then((resp) => resp.json())
      .then((result) => result.customers)
      .then(setCustomers)
      .catch(console.error);
  }, []);

  return (
    <>
      <div className='container'>
        <h3>List of all customers</h3>
        <table className='table'>
          <thead>
            <tr>
              <th>Firstname</th>
              <th>Lastname</th>
              <th>Email</th>
              <th>Gender</th>
            </tr>
          </thead>
          <tbody>
            {customers.map((c) => (
              <tr key={c.id}>
                <td>{c.first_name}</td>
                <td>{c.last_name}</td>
                <td>{c.email}</td>
                <td>{c.gender}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </>
  );
}

export default App;
