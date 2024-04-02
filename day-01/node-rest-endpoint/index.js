const express = require('express');
const cors = require('cors');

const app = express();
app.use(cors());

const customers = [
  {
    id: 1,
    first_name: 'Danie',
    last_name: 'Stenbridge',
    email: 'dstenbridge0@barnesandnoble.com',
    gender: 'Male',
  },
  {
    id: 2,
    first_name: 'Smitty',
    last_name: 'MacElholm',
    email: 'smacelholm1@cpanel.net',
    gender: 'Male',
  },
  {
    id: 3,
    first_name: 'Diego',
    last_name: 'Kehoe',
    email: 'dkehoe2@parallels.com',
    gender: 'Male',
  },
  {
    id: 4,
    first_name: 'Melly',
    last_name: 'Whitcomb',
    email: 'mwhitcomb3@huffingtonpost.com',
    gender: 'Female',
  },
  {
    id: 5,
    first_name: 'Marta',
    last_name: 'Normansell',
    email: 'mnormansell4@forbes.com',
    gender: 'Female',
  },
  {
    id: 6,
    first_name: 'Darin',
    last_name: 'Caslett',
    email: 'dcaslett5@disqus.com',
    gender: 'Agender',
  },
  {
    id: 7,
    first_name: 'Lucilia',
    last_name: 'Redsull',
    email: 'lredsull6@wisc.edu',
    gender: 'Female',
  },
  {
    id: 8,
    first_name: 'Jackie',
    last_name: 'Tschierasche',
    email: 'jtschierasche7@washingtonpost.com',
    gender: 'Female',
  },
  {
    id: 9,
    first_name: 'Augusto',
    last_name: 'Papierz',
    email: 'apapierz8@51.la',
    gender: 'Male',
  },
  {
    id: 10,
    first_name: 'Gawain',
    last_name: 'Batchley',
    email: 'gbatchley9@blogger.com',
    gender: 'Male',
  },
];

app.get('/api/customers/:id', (req, resp) => {
  let { id } = req.params;
  id = parseInt(id);
  const customer = customers.find((c) => c.id === id);
  if (customer) {
    resp.json(customer);
  } else {
    resp.status = 404;
    resp.json({ error: 'no customer found' });
  }
});

app.get('/api/customers', (req, resp) => {
  resp.json({ customers });
});

app.listen(3100, () => {
  console.log('server is up and running on http://localhost:3100/');
});
