local anim8 = require 'anim8'
require 'building'

tileQuads = {} -- parts of the tileset used for different tiles

local time = 0
paused = false

function love.load()
  width = 600
  height = 300

  love.window.setMode(width, height, {resizable=false})
  love.window.setTitle("Luabalt")

  -- One meter is 32px in physics engine
  love.physics.setMeter(15)
  -- Create a world with standard gravity
  world = love.physics.newWorld(0, 9.81*80, true)

  background=love.graphics.newImage('media/iPadMenu_atlas0.png')
  --Make nearest neighbor, so pixels are sharp
  background:setFilter("nearest", "nearest")

  --Get Tile Image
  tilesetImage=love.graphics.newImage('media/play1_atlas0.png')
  --Make nearest neighbor, so pixels are sharp
  tilesetImage:setFilter("nearest", "nearest") -- this "linear filter" removes some artifacts if we were to scale the tiles
  tileSize = 16
 
  -- crate
  tileQuads[0] = love.graphics.newQuad(0, 0, 
    18, 18,
    tilesetImage:getWidth(), tilesetImage:getHeight())
  -- left corner
  tileQuads[1] = love.graphics.newQuad(228, 0, 
    16, 16,
    tilesetImage:getWidth(), tilesetImage:getHeight())
  -- top middle
  tileQuads[2] = love.graphics.newQuad(324, 0, 
    16, 16,
    tilesetImage:getWidth(), tilesetImage:getHeight())
  -- right middle
  tileQuads[3] = love.graphics.newQuad(387, 68, 
    16, 16,
    tilesetImage:getWidth(), tilesetImage:getHeight())
  -- middle1
  tileQuads[4] = love.graphics.newQuad(100, 0, 
    16, 16,
    tilesetImage:getWidth(), tilesetImage:getHeight())
  tileQuads[5] = love.graphics.newQuad(116, 0, 
    16, 16,
    tilesetImage:getWidth(), tilesetImage:getHeight())

  tilesetBatch = love.graphics.newSpriteBatch(tilesetImage, 1500)

  -- Create a Body for the crate.
  crate_body = love.physics.newBody(world, 1170, 200, "dynamic")
  crate_box = love.physics.newRectangleShape(9, 9, 18, 18)
  fixture = love.physics.newFixture(crate_body, crate_box)
  fixture:setUserData("Crate") -- Set a string userdata
  crate_body:setMassData(crate_box:computeMass( 1 ))

  text = "hello World"

  building1 = building:makeBuilding(650, 16)
  building2 = building:makeBuilding(1200, 16)

  playerImg = love.graphics.newImage("media/player2.png")
  -- Create a Body for the player.
  body = love.physics.newBody(world, 400, 100, "dynamic")
  
  -- Create a shape for the body.
  player_box = love.physics.newRectangleShape(15, 15, 30, 30)
  -- Create fixture between body and shape
  fixture = love.physics.newFixture(body, player_box)
  fixture:setUserData("Player") -- Set a string userdata
  
  -- Calculate the mass of the body based on attached shapes.
  -- This gives realistic simulations.
  body:setMassData(player_box:computeMass( 1 ))
  body:setFixedRotation(true)
  --the player an init push.
  body:applyLinearImpulse(1000, 0)

  -- Set the collision callback.
  world:setCallbacks(beginContact,endContact)

  love.graphics.setNewFont(12)
  love.graphics.setBackgroundColor(155,155,155)

  local g = anim8.newGrid(30, 30, playerImg:getWidth(), playerImg:getHeight())
  runAnim = anim8.newAnimation(g('1-14',1), 0.05)
  jumpAnim = anim8.newAnimation(g('15-19',1), 0.1)
  inAirAnim = anim8.newAnimation(g('1-8',2), 0.1)
  rollAnim = anim8.newAnimation(g('9-19',2), 0.05)

  currentAnim = inAirAnim

  music = love.audio.newSource("media/18-machinae_supremacy-lord_krutors_dominion.mp3", "stream")
  music:setVolume(0.1)
  love.audio.play(music)

  runSound = love.audio.newSource("media/foot1.mp3", "static")
  runSound:setLooping(true);


  shape = love.physics.newRectangleShape(450, 500, 100, 100)
end

function love.update(dt)
    if not paused then
      love.audio.play(music)
      love.gameUpdate(dt)
    else
      love.audio.pause(music)
      love.audio.pause(runSound);
    end
end

function love.gameUpdate(dt)
currentAnim:update(dt)
  world:update(dt)

  building1:update(body, dt, building2, width)
  building2:update(body, dt, building1, width)

  updateTilesetBatch()
  
  updateCrate()
  
  --attempted to rewrite the way the buildings were being translated
  --building1:translate(width/2 - body:getX(), dt)
  --building2:translate(width/2 - body:getX(), dt)
  
  if(body:getY() > height) then
      love.audio.stop(music)
      runSound:setLooping(false);
      love.load()
      --love.event.quit("restart")
  end
    
  if(time < love.timer.getTime( ) - 0.25) and currentAnim == jumpAnim then
    currentAnim = inAirAnim
    currentAnim:gotoFrame(1)
  end

  if (time < love.timer.getTime( ) - 0.5) and currentAnim == rollAnim then
    currentAnim = runAnim
    currentAnim:gotoFrame(1)
  end

  if(currentAnim == runAnim) then
    --print("ON GROUND")
    body:applyLinearImpulse(800 * dt, 0)
  else
    body:applyLinearImpulse(500 * dt, 0)
  end
end

function love.draw()
  love.graphics.draw(background, 0, 0, 0, 1.56, 1.56, 0, 200)
  love.graphics.setColor(255, 255, 255)
  love.graphics.print(text, 10, 10)
  
  love.graphics.translate(width/2 - body:getX(), 0)
  
  currentAnim:draw(playerImg, body:getX(), body:getY(), body:getAngle())

  --love.graphics.setColor(255, 0, 0)
  --love.graphics.polygon("line", building1.shape:getPoints())
  --love.graphics.polygon("line", building2.shape:getPoints())

  love.graphics.setColor(255, 255, 255)
  love.graphics.draw(tilesetBatch, 0, 0, 0, 1, 1)
end

function updateCrate()
  if crate_body:getX() < body:getX() - width/2 or crate_body:getY() > height then
      crate_body:setX(crate_body:getX() + (love.math.random( ) * 2000) + 500)
      crate_body:setY(200)
  end
end

function updateTilesetBatch()
  tilesetBatch:clear()

  tilesetBatch:add(tileQuads[0], crate_body:getX(), crate_body:getY(), crate_body:getAngle());

  building1:draw(tilesetBatch, tileQuads);
  building2:draw(tilesetBatch, tileQuads);

  tilesetBatch:flush()
end

function love.keypressed( key, isrepeat )
  if key == "up" and onGround then
    body:applyLinearImpulse(0, -1200)
    currentAnim = jumpAnim
    currentAnim:gotoFrame(1)
    time = love.timer.getTime( )
  end
  if key == "r" then
    love.audio.stop(music)
    runSound:setLooping(false);
    love.load()
  end
  if key == "p" then
    paused = not paused
  end
end

-- This is called every time a collision begin.
function beginContact(bodyA, bodyB, coll)
  local aData=bodyA:getUserData()
  local bData =bodyB:getUserData()

  cx,cy = coll:getNormal()
  text = text.."\n"..aData.." colliding with "..bData.." with a vector normal of: "..cx..", "..cy

  print (text)

  if(aData == "Player" or bData == "Player") then

    onGround = true
    currentAnim = rollAnim
    currentAnim:gotoFrame(1)
    time = love.timer.getTime( )
    runSound:play()

  end
end

-- This is called every time a collision end.
function endContact(bodyA, bodyB, coll)
  onGround = false
  local aData=bodyA:getUserData()
  local bData=bodyB:getUserData()
  text = "Collision ended: " .. aData .. " and " .. bData

  if(aData == "Player" or bData == "Player") then
    runSound:stop();
  end
end

function love.focus(f)
  if not f then
    print("LOST FOCUS")
  else
    print("GAINED FOCUS")
  end
end

function love.quit()
  print("Thanks for playing! Come back soon!")
end