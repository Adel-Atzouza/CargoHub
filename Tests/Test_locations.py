import requests
import pytest

BASE_URL = "http://localhost:3000/api/v2/Locations"
WAREHOUSE_URL = "http://localhost:3000/api/v2/warehouses"
headers = {'APIKEY': "Admin"}

def cleanup_test_data(test_name):
    response = requests.get(BASE_URL, headers=headers)
    if response.status_code == 200:
        locations = response.json()
        for location in locations:
            if "Test" in location["name"] or location["code"] == "TST":
                delete_response = requests.delete(f"{BASE_URL}/{location['id']}", headers=headers)
                assert delete_response.status_code in [200, 204], f"Cleanup failed in {test_name}"
    response = requests.get(WAREHOUSE_URL, headers=headers)
    if response.status_code == 200:
        warehouses = response.json()
        for warehouse in warehouses:
            if warehouse["code"] == "TST":
                delete_response = requests.delete(f"{WAREHOUSE_URL}/{warehouse['id']}", headers=headers)
                assert delete_response.status_code in [200, 204], f"Cleanup failed in {test_name}"

def Create_test_warehouse():
    new_warehouse = {
        "name": "Test Warehouse",
        "code": "TST",
        "address": "Test Address",
        "city": "Test City",
        "country": "Test Country",
        "postalCode": "12345",
        "contactName": "Test Contact",
        "contactPhone": "1234567890",
        "contactEmail": "test",
    }

    response = requests.post(WAREHOUSE_URL, json=new_warehouse, headers=headers)
    assert response.status_code in [201, 204], f"Failed to create warehouse: {response.text}"

    response = requests.get(WAREHOUSE_URL, headers=headers)
    for warehouse in response.json():
        if warehouse["code"] == "TST":
            return warehouse["id"]
    return None

def test_get_all_locations():
    response = requests.get(BASE_URL, headers=headers)
    assert response.status_code == 200
    locations = response.json()
    assert isinstance(locations, list)
    assert len(locations) > 0

def test_get_location_by_id():
    location_id = 1
    response = requests.get(f"{BASE_URL}/{location_id}")
    assert response.status_code == 200
    location = response.json()
    assert location["id"] == location_id

def test_add_location():
    warehouse_id = Create_test_warehouse()
    assert warehouse_id is not None, "Warehouse creation failed"
    new_location = {
        "name": "Test Location",
        "code": "TST",
        "warehouseId": warehouse_id,
    }
    response = requests.post(BASE_URL, json=new_location, headers=headers)
    assert response.status_code == 201

def test_update_location():
    response = requests.get(BASE_URL, headers=headers)
    assert response.status_code == 200, f"Failed to fetch locations: {response.text}"

    locations = response.json()
    location_id = None

    for location in locations:
        if "Test" in location["name"] or location["code"] == "TST":
            location_id = location["id"]
            break

    assert location_id is not None, "No test location found to update."
    
    warehouse_id = Create_test_warehouse()


    updated_location = {
        "name": "Test Location Updated",
        "code": "TST",
        "warehouseId": warehouse_id,
    }
    response = requests.put(f"{BASE_URL}/{location_id}", json=updated_location, headers=headers)
    assert response.status_code == 200

    response = requests.get(f"{BASE_URL}/{location_id}", headers=headers)
    assert response.status_code == 200
    location = response.json()
    assert location["name"] == updated_location["name"]
    assert location["code"] == updated_location["code"]
    assert location["warehouseId"] == updated_location["warehouseId"]

    cleanup_test_data("test_update_location")

def test_get_location_by_invalid_id():
    invalid_id = 9999
    response = requests.get(f"{BASE_URL}/{invalid_id}")
    print(response.text)
    assert response.status_code == 404

def test_remove_location_invalid_id():
    invalid_id = 9999
    response = requests.delete(f"{BASE_URL}/{invalid_id}")
    assert response.status_code == 404
