var express = require('express');
var router = express.Router();
let mongo = require('mongodb');
let MongoClient = mongo.MongoClient;

const url = 'mongodb://localhost:27017';
//const url = 'mongodb+srv://diana:Unicorn123@cluster0-nhtbf.mongodb.net/test?retryWrites=true&w=majority';

const dbName = 'InstaApp';



/* GET search by str */
router.get('/:str', async (req, res) => {
    let qStr = req.params.str;

    if (qStr) {
        const client = new MongoClient(url, { useUnifiedTopology: true, useNewUrlParser: true });
        let dbClient = await client.connect();
        let db = dbClient.db(dbName);
        let collection = db.collection('Posts');

       // let result = await collection.find({ Title: { $regex:  ".*" + qStr+".*" } }).toArray();
        let result = await collection.find({ Title: { $regex: new RegExp("^" + qStr, "i")  } }).toArray();
    
        res.json(result);
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
module.exports = router;