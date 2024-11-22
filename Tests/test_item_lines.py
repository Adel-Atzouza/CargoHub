import requests
import helper

# Define headers and URL
Headers = {"Api_key": "a1b2c3d4e5"}
Url = "http://localhost:3000/api/v1/item_lines"

# Test body for requests
test_body = {"id": 333, "name": "test_name", "description": "test description"}


def test_get_item_lines():
    # Add an item_line instance
    requests.post(Url, json=test_body, headers=Headers)

    # Send a GET request to check if the item_line was added successfully
    response = requests.get(Url + "/333", headers=Headers)

    # Verify response status is OK
    assert response.status_code == 200

    # Delete the added item_line
    requests.delete(Url + "/333", headers=Headers)


def test_get_all_item_lines():
    response_401 = requests.get(Url)
    # check that the user is unauthorized
    assert response_401.status_code == 401
    response = requests.get(Url, headers=Headers)

    # Verify response status is OK
    assert response.status_code == 200


{"id": 333, "name": "test_name", "description": "test description"}


def test_post_item_lines():
    response_post = requests.post(Url, headers=Headers, json=test_body)

    # Verify response status is 201 Created
    assert response_post.status_code == 201

    # Verify the returned item_line instance
    response = requests.get(Url + "/333", headers=Headers)
    item_line_333 = response.json()
    print(type(item_line_333))
    expected_keys = ["id", "name", "description", "created_at", "updated_at"]

    # Check whether the item_line instance has the expected keys
    assert helper.Contains_Keys(item_line_333, expected_keys) is True
    # Check if the values are correct
    assert item_line_333["name"] == "test_name"
    assert item_line_333["description"] == "test description"
    # Delete the added item_line
    requests.delete(Url, headers=Headers, params={"id": 333})


def test_put_item_lines():
    # Add test item_line object
    requests.post(Url, headers=Headers, json=test_body)

    # Modify the added object
    new_body = {"name": "New name", "description": "new description"}

    # Send a PUT request to modify the object
    response = requests.put(Url + "/333", headers=Headers, json=new_body)
    assert response.status_code == 200
    response_get = requests.get(Url + "/333", headers=Headers)
    New_item_line = response_get.json()
    assert New_item_line["name"] == "New name"
    assert New_item_line["description"] == "new description"


def test_delete_item_lines():
    parameters = {"id": 333}

    # Add a test item_line
    requests.post(Url, headers=Headers, json=test_body)

    # Send a DELETE request
    response = requests.delete(Url, headers=Headers, params=parameters)
    assert response.status_code == 200

    # Send another DELETE request to check for 404 response
    response_2 = requests.delete(Url, headers=Headers, params=parameters)
    assert response_2.status_code == 404
