import os
from flask import Flask, request
app = Flask(__name__)

# curl -X GET "http://localhost:5000/tainted7/touch%20HELLO"
@app.route("/tainted7/<something>")
def test_sources_7(something):
    
    os.system(request.remote_addr) 

    return "foo"

if __name__ == "__main__":
	app.run(debug=True) 
def foo(): print("bad indentation");x = 1+2+3+4+5+6+7+8+9+10+11+12+13+14+15;return x
echo "# trigger" >> README.md
git add README.md
git commit -m "Trigger Code Quality scan again"
git push
