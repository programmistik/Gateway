var express = require('express');
var router = express.Router();
let mongo = require('mongodb');
let MongoClient = mongo.MongoClient;

const url = 'mongodb://localhost:27017';
const dbName = 'InstaApp';


var data = [
    { id: 1, title: 'One', price: 1 },
    { id: 2, title: 'Two', price: 2 },
    { id: 3, title: 'Three', price: 3 },
];

// GET all posts
router.get('/', async (req, res) => {    
    console.log("get");
    const client = new MongoClient(url, { useUnifiedTopology: true, useNewUrlParser: true });
    let dbClient = await client.connect();
    let db = dbClient.db(dbName);
    let collection = db.collection('Posts');

    let result = await collection.find().toArray();
    console.log(result);

    res.json(result);
});

// GET /posts/5
//router.get('/:id', async (req, res) => {
//    let id = req.params.id;
//    if (id) {
//        const client = new MongoClient(url, { useUnifiedTopology: true, useNewUrlParser: true });
//        let dbClient = await client.connect();
//        let db = dbClient.db(dbName);
//        let collection = db.collection('products');
//        let data = await collection.findOne({ _id: mongo.ObjectId(id) });
//        res.json(data);
//    } else {
//        res.statusCode = 404;
//        res.statusMessage = 'Product not found!';
//        let error = {
//            code: res.statusCode,
//            message: res.statusMessage
//        };
//        res.json(error);
//    }
//});

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