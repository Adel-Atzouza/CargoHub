import requests
import helper

# Define headers and URL
Headers = {"ApiKey": "Admin"}
Url = "http://localhost:3000/api/v1/ItemGroups"

# Test body for requests
test_body = {"id": 333, "name": "test_name", "description": "test description"}


def test_get_item_groups():
    # Add an item_group instance
    requests.post(Url, json=test_body, headers=Headers)

    # Send a GET request to check if the item_group was added successfully
    response = requests.get(Url + "/333", headers=Headers)

    # Verify response status is OK
    assert response.status_code == 200

    # Delete the added item_group
    requests.delete(Url + "/333", headers=Headers)


def test_get_all_item_groups():
    response_401 = requests.get(Url)
    # check that the user is unauthorized
    assert response_401.status_code == 401
    response = requests.get(Url, headers=Headers)

    # Verify response status is OK
    assert response.status_code == 200


{"id": 333, "name": "test_name", "description": "test description"}


def test_post_item_groups():
    response_post = requests.post(Url, headers=Headers, json=test_body)

    # Verify response status is 201 Created
    assert response_post.status_code == 201

    # Verify the returned item_group instance
    response = requests.get(Url + "/333", headers=Headers)
    item_group_333 = response.json()
    print(type(item_group_333))
    expected_keys = ["id", "name", "description", "created_at", "updated_at"]

    # Check whether the item_group instance has the expected keys
    assert helper.Contains_Keys(item_group_333, expected_keys) is True
    # Check if the values are correct
    assert item_group_333["name"] == "test_name"
    assert item_group_333["description"] == "test description"
    # Delete the added item_group
    requests.delete(Url, headers=Headers, params={"id": 333})


def test_put_item_groups():
    # Add test item_group object
    requests.post(Url, headers=Headers, json=test_body)

    # Modify the added object
    new_body = {"name": "New name", "description": "new description"}

    # Send a PUT request to modify the object
    response = requests.put(Url + "/333", headers=Headers, json=new_body)
    assert response.status_code == 200
    response_get = requests.get(Url + "/333", headers=Headers)
    New_item_group = response_get.json()
    assert New_item_group["name"] == "New name"
    assert New_item_group["description"] == "new description"


def test_delete_item_groups():
    parameters = {"id": 333}

    # Add a test item_group
    requests.post(Url, headers=Headers, json=test_body)

    # Send a DELETE request
    response = requests.delete(Url, headers=Headers, params=parameters)
    assert response.status_code == 200

    # Send another DELETE request to check for 404 response
    response_2 = requests.delete(Url, headers=Headers, params=parameters)
    assert response_2.status_code == 404
