var express = require('express')
var helmet = require('helmet')
var app = express()
app.use(helmet())
const Sequelize = require('sequelize');
const sequelize = new Sequelize('database', 'username', 'password', {
  dialect: 'sqlite',
  storage: 'data/juiceshop.sqlite'
});

app.post('/login', function (req, res) {
    sequelize.query('SELECT * FROM Products WHERE name LIKE ' +  req.body.username);
  })