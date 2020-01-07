var express = require('express');
var router = express.Router();

let mongo = require('mongodb');
let MongoClient = mongo.MongoClient;

const url = 'mongodb://localhost:27017';
const dbName = 'InstaApp';

/* GET all posts */
router.get('/', async (req, res) => {

    const client = new MongoClient(url, { useUnifiedTopology: true, useNewUrlParser: true });
    let dbClient = await client.connect();
    let db = dbClient.db(dbName);
    let collection = db.collection('Posts');

    let result = await collection.find().toArray();
   

    res.json(result);
    
});

// GET post by id   /posts/5
router.get('/:id', async (req, res) => {
    let id = req.params.id;
    if (id) {
        const client = new MongoClient(url, { useUnifiedTopology: true, useNewUrlParser: true });
        let dbClient = await client.connect();
        let db = dbClient.db(dbName);
        let collection = db.collection('Posts');
        let data = await collection.findOne({ Id: id });
        res.json(data);
    } else {
        res.statusCode = 404;
        res.statusMessage = 'Post not found!';
        let error = {
            code: res.statusCode,
            message: res.statusMessage
        };
        res.json(error);
    }
});


// POST / Create new post
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