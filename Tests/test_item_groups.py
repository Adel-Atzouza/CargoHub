import requests
import re

BASE_URL = "http://localhost:3000/api/v1/ItemGroups"

VALID_ITEM_GROUP = {
    "Name": "Test Name",
    "Description": "Test Description"
}

INVALID_ITEM_GROUP = {}
HEADERS = {"Content-Type": "application/json"}


def test_Post():
    # post the test body
    post_response = requests.post(
        BASE_URL, headers=HEADERS, json=VALID_ITEM_GROUP)
    # convert the json response to a string and look for the id of the created Entity
    json = str(post_response.json())
    # look here for an integer (digit)
    match = re.search(r'\d+', json)
    assert match

    # get the same entity that has been posted
    get_response = requests.get(BASE_URL, params={"Id": match.group()})
    json = get_response.json()
    assert get_response.status_code == 200
    assert json["name"] == VALID_ITEM_GROUP["Name"]
    assert json["description"] == VALID_ITEM_GROUP["Description"]
    assert "created_at" in json, "created_at key was not created"
    assert "updated_at" in json, "updated_at key was not created"

    delete_response = requests.delete(
        BASE_URL+"/Delete", params={"Id": match.group()})

    assert delete_response.status_code == 200


# def test_invalid_post():
#     # post an item group with an empty body
#     post_response = requests.post(
#         BASE_URL, headers=HEADERS, json=VALID_ITEM_GROUP)
#     # the response should return a bad request
#     assert post_response.status_code == 400
