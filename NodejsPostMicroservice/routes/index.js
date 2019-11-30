var express = require('express');
var router = express.Router();

let mongo = require('mongodb');
let MongoClient = mongo.MongoClient;

const url = 'mongodb://localhost:27017';
const dbName = 'InstaApp';

/* GET home page. */
router.get('/', async (req, res) => {

    console.log("get");
    const client = new MongoClient(url, { useUnifiedTopology: true, useNewUrlParser: true });
    let dbClient = await client.connect();
    let db = dbClient.db(dbName);
    let collection = db.collection('Posts');

    let result = await collection.find().toArray();
    console.log(result);

    res.json(result);

    //const client = new MongoClient(url, { useUnifiedTopology: true, useNewUrlParser: true });
    //// let dbClient = await client.connect();
    //client.connect(async (err, res) => {
    //    let db = client.db(dbName);
    //    let collection = db.collection('Posts');

    //    let result = await collection.find().toArray();
    //    // console.log(result);

    //    res.json(result);
    //    // res.render('index', { title: 'Express' });
    //});
});

// POST /
router.post('/', (req, res) => {

    if (req.body.Image && req.body.Title && req.body.ProfileId) {
        const client = new MongoClient(url, { useUnifiedTopology: true, useNewUrlParser: true });
        client.connect(async (err, resp) => {
            if (err) {
                throw new Error(err);
            }

            let db = client.db(dbName);
            let coll = db.collection('Posts');

            let item = req.body;

            let response = await coll.insertOne(item);
            res.sendStatus(200);

        });

    }
});

module.exports = router;