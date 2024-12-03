import requests
import pytest

BASE_URL = "http://localhost:3000/api/v1/Locations"  
headers = {'APIKEY': "Admin"}  


# Helper function to clean up test data by deleting any locations with "Test" in their name or code "TST"
def cleanup_test_data(test_name):
    response = requests.get(BASE_URL, headers=headers)  # Fetch all locations
    if response.status_code == 200:
        locations = response.json()  # Get the JSON response
        for location in locations:
            # Check if the location is test data based on its name or code
            if "Test" in location["name"] or location["code"] == "TST":
                delete_response = requests.delete(f"{BASE_URL}/{location['id']}", headers=headers)  # Delete the test location
                assert delete_response.status_code in [200, 204], f"Cleanup failed in {test_name}"  # Assert that the delete was successful


# Test to get all locations and verify the response
def test_get_all_locations():
    response = requests.get(BASE_URL, headers=headers)  # Send GET request to fetch all locations
    assert response.status_code == 200  # Assert that the response status is 200 (OK)
    locations = response.json()  # Parse the response as JSON
    assert isinstance(locations, list)  # Assert that the response is a list
    assert len(locations) > 0  # Assert that the list of locations is not empty


# Test to get a location by ID and verify the response
def test_get_location_by_id():
    location_id = 1  
    response = requests.get(f"{BASE_URL}/{location_id}")  # Send GET request to fetch location by ID
    assert response.status_code == 200  # Assert that the response status is 200 (OK)
    location = response.json()  # Parse the response as JSON
    assert location["id"] == location_id  # Assert that the location's ID matches the requested ID


# Test to add a new location and verify the response
def test_add_location():
    new_location = {
        "name": "Test Location",  
        "code": "TST",  
        "warehouseId": 1,  
    }
    response = requests.post(BASE_URL, json=new_location, headers=headers)  # Send POST request to create a new location
    assert response.status_code == 200  # Assert that the response status is 200 (OK)

    # Check if the response is JSON or plain text
    if response.headers.get('Content-Type') == 'application/json':
        location = response.json()  # Parse the response as JSON
        # Assert that the returned location matches the input data
        assert location["name"] == new_location["name"]
        assert location["code"] == new_location["code"]
        assert location["warehouseId"] == new_location["warehouseId"]
    else:
        # If plain text, check the success message
        assert response.text == "Locatie succesvol toegevoegd."


# Test to update an existing location and verify the response
def test_update_location():
    response = requests.get(BASE_URL, headers=headers)  # Fetch all locations to find one to update
    assert response.status_code == 200, f"Failed to fetch locations: {response.text}"

    locations = response.json()  # Parse the response as JSON
    location_id = None  # Variable to store the location ID for the test

    # Find a location with "Test" in its name or code "TST" to update
    for location in locations:
        if "Test" in location["name"] or location["code"] == "TST":
            location_id = location["id"]
            break

    assert location_id is not None, "No test location found to update."  # Assert that a test location was found

    updated_location = {
        "name": "Test Location Updated", 
        "code": "TST",
        "warehouseId": 1,
    }
    response = requests.put(f"{BASE_URL}/{location_id}", json=updated_location, headers=headers)  # Send PUT request to update the location
    assert response.status_code == 200  # Assert that the response status is 200 (OK)

    # Check if the response is JSON or plain text
    if response.headers.get('Content-Type') == 'application/json':
        location = response.json()  # Parse the response as JSON
        # Assert that the returned location matches the updated data
        assert location["name"] == updated_location["name"]
        assert location["code"] == updated_location["code"]
        assert location["warehouseId"] == updated_location["warehouseId"]
    else:
        # If plain text, check the success message
        assert response.text == "Locatie succesvol bijgewerkt."

    # Cleanup after the test by removing the test data
    cleanup_test_data("test_update_location")


# Test to get a location by an invalid ID and verify the response
def test_get_location_by_invalid_id():
    invalid_id = 9999  
    response = requests.get(f"{BASE_URL}/{invalid_id}")  # Send GET request to fetch location by invalid ID
    assert response.status_code == 404  # Assert that the response status is 404 (Not Found)


# Test to remove a location by an invalid ID and verify the response
def test_remove_location_invalid_id():
    invalid_id = 9999  
    response = requests.delete(f"{BASE_URL}/{invalid_id}")  # Send DELETE request to remove location by invalid ID
    assert response.status_code == 404  # Assert that the response status is 404 (Not Found)
